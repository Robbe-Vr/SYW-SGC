import base64
from datetime import datetime, timedelta
from io import BytesIO
from typing import Any, Optional
from PIL import Image
import numpy as np

import string
import secrets
alphabet = string.ascii_letters + string.digits

import cv2
from mediapipe.python.solutions.holistic import Holistic
from mediapipe.framework.formats.landmark_pb2 import NormalizedLandmark
from mediapipe.framework.formats.landmark_pb2 import NormalizedLandmarkList

class HolisticCoordsResult:
    x: float
    y: float
    z: float

class HolisticTrackingResult:
    def __setitem__(self, key, value):
        setattr(self, key, value)

    def __getitem__(self, key):
        return getattr(self, key)

class HolisticResult:
    pose: Optional[HolisticTrackingResult]
    pose_world: Optional[HolisticTrackingResult]
    face: Optional[HolisticTrackingResult]
    left_hand: Optional[HolisticTrackingResult]
    right_hand: Optional[HolisticTrackingResult]
    segmentation_mask: Any

holistic = None

class TokenInfo:
    clientName: str
    token: str
    lastActive: datetime

security_tokens: list[TokenInfo] = []

def generateToken(length: int):
    while True:
        password = ''.join(secrets.choice(alphabet) for i in range(length))
        if (any(c.islower() for c in password)
                and any(c.isupper() for c in password)
                and sum(c.isdigit() for c in password) >= 3):
            break

    return password

def cleanUpSecurityTokens():
    global security_tokens
    cleanedUpTokens: list[TokenInfo] = []
    for token in security_tokens:
        if (token.lastActive + timedelta(hours=3) > datetime.now()):
            cleanedUpTokens.append(token)

    security_tokens = cleanedUpTokens

def clientAlreadyConnected(connectedClient: TokenInfo, newClient: str):
    return connectedClient.clientName == newClient

def connect(clientName: str):
    global security_tokens
    
    if (security_tokens.__len__() > 0 and any(clientAlreadyConnected(connectedClient, clientName)) for connectedClient in security_tokens):
        print("Reconnecting client " + clientName + "...")
        disconnect(clientName)

    print("New client connected: " + clientName)

    tokenInfo = TokenInfo()
    tokenInfo.clientName = clientName
    tokenInfo.token = generateToken(21)
    tokenInfo.lastActive = datetime.now()

    security_tokens.append(tokenInfo)

    global holistic

    if (holistic == None):
        holistic = Holistic(
            min_detection_confidence=0.5,
            min_tracking_confidence=0.5)
    
    return tokenInfo.token

def disconnect(clientName: str):
    global security_tokens
    global holistic
    try:
        security_tokens = filter(lambda token: token.clientName != clientName, security_tokens)

        print("Client disconnected: " + clientName)

        cleanUpSecurityTokens()

        if (security_tokens.__len__() < 1):
            print('No active clients! shutting down process until a new client connects...')
            holistic.close()
            holistic = None
    except:
        print('unknown token.')

def validateSecurityToken(securityToken: str, token: str):
    return securityToken.token == token

def process_image(token: str, base64EncodedImage: str, timestamp: int, width: int, height: int):
    global security_tokens
    if (not any(validateSecurityToken(securityToken, token) for securityToken in security_tokens)):
        return { "message": "invalid token!" }

    for securityToken in security_tokens:
        if (securityToken.token == token):
            securityToken.lastActive = datetime.now()
            break

    global holistic
    if (holistic == None):
        return { "message": "client has not yet connected!" }

    try:
        image = cv2.cvtColor(cv2.Mat(image_base64_into_numpy_array(base64EncodedImage, width, height)), cv2.COLOR_BGR2RGB)

        image.flags.writeable = False

        results = holistic.process(image)

        response_result = HolisticResult()
        if results.pose_landmarks:
            response_result.pose = formatLandmarksToTrackingResult(results.pose_landmarks.landmark, index_to_name_list=index_to_name_pose)
        if results.pose_world_landmarks:
            response_result.pose_world = formatLandmarksToTrackingResult(results.pose_world_landmarks.landmark, index_to_name_list=[])
        if results.face_landmarks:
            response_result.face = formatLandmarksToTrackingResult(results.face_landmarks.landmark, index_to_name_list=[])
        if results.left_hand_landmarks:
            response_result.left_hand = formatLandmarksToTrackingResult(results.left_hand_landmarks.landmark, index_to_name_list=index_to_name_hand)
        if results.right_hand_landmarks:
            response_result.right_hand = formatLandmarksToTrackingResult(results.right_hand_landmarks.landmark, index_to_name_list=index_to_name_hand)
        if results.segmentation_mask:
            response_result.segmentation_mask = results.segmentation_mask

        return response_result
    except Exception as e:
        print('Unable to process image! Error: {}'.format(repr(e)))
        return {}

def image_base64_into_numpy_array(data: str, width: int, height: int):
    np_array = np.array(Image.open(BytesIO(base64.b64decode(data))))
    np_array = np_array.reshape(width, height)
    return np_array

index_to_name_pose = [
    'nose',
    'left_eye_inner', 'left_eye', 'left_eye-outer', 'right_eye_inner', 'right_eye', 'right_eye-outer',
    'left_ear', 'right_ear',
    'mouth_left', 'mouth_right',
    'left_shoulder', 'right_shoulder',
    'left_elbow', 'right_elbow',
    'left_wrist', 'right_wrist',
    'left_pinky', 'right_pinky',
    'left_index', 'right_index',
    'left_thumb', 'right_thumb',
    'left_hip', 'right_hip',
    'left_knee', 'right_knee',
    'left_ankle', 'right_ankle',
    'left_heel', 'right_heel',
    'left_foot_index', 'right_foot_index'
]

index_to_name_hand = [
    'wrist',
    'thumb_cmc', 'thumb_mcp', 'thumb_ip', 'thumb_tip',
    'index_finger_mcp', 'index_finger_pip', 'index_finger_dip', 'index_finger_tip',
    'middle_finger_mcp', 'middle_finger_pip', 'middle_finger_dip', 'middle_finger_tip',
    'ring_finger_mcp', 'ring_finger_pip', 'ring_finger_dip', 'ring_finger_tip',
    'pinky_mcp', 'pinky_pip', 'pinky_dip', 'pinky_tip',
]

def formatLandmarksToTrackingResult(landmarks: NormalizedLandmarkList, index_to_name_list: list[str]) -> HolisticTrackingResult:
    tracked = HolisticTrackingResult()

    if index_to_name_list.__len__() == 0:
        landmarksList: list[HolisticCoordsResult] = []

        for i in landmarks:
            coords = formatSingleLandmarkToCoords(i)
            landmarksList.append(coords)

        tracked.list = landmarksList
    else:
        index = 0
        for i in landmarks:
            coordsName = index_to_name_list[index]
            tracked.__setitem__(coordsName, formatSingleLandmarkToCoords(i))
            index += 1

    return tracked

def formatSingleLandmarkToCoords(singleLandmark: NormalizedLandmark) -> HolisticCoordsResult:
    coords = HolisticCoordsResult()

    coords.x = singleLandmark.x
    coords.y = singleLandmark.y
    coords.z = singleLandmark.z

    return coords
