from mediapipe.python.solutions.holistic import PoseLandmark

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

print('nose: {}'.format(PoseLandmark.NOSE))
print('left eye inner: {}'.format(PoseLandmark.LEFT_EYE_INNER))
print('left eye: {}'.format(PoseLandmark.LEFT_EYE))
print('left eye outer: {}'.format(PoseLandmark.LEFT_EYE_OUTER))
print('right eye inner: {}'.format(PoseLandmark.RIGHT_EYE_INNER))
print('right eye: {}'.format(PoseLandmark.RIGHT_EYE))
print('right eye outer: {}'.format(PoseLandmark.RIGHT_EYE_OUTER))
print('left ear: {}'.format(PoseLandmark.LEFT_EAR))
print('right ear: {}'.format(PoseLandmark.RIGHT_EAR))
print('mouth left: {}'.format(PoseLandmark.MOUTH_LEFT))
print('mouth right: {}'.format(PoseLandmark.MOUTH_RIGHT))
print('left shoulder: {}'.format(PoseLandmark.LEFT_SHOULDER))
print('right shoulder: {}'.format(PoseLandmark.RIGHT_SHOULDER))
print('left elbow: {}'.format(PoseLandmark.LEFT_ELBOW))
print('right elbow: {}'.format(PoseLandmark.RIGHT_ELBOW))
print('left wrist: {}'.format(PoseLandmark.LEFT_WRIST))
print('right wrist: {}'.format(PoseLandmark.RIGHT_WRIST))
print('left pinky: {}'.format(PoseLandmark.LEFT_PINKY))
print('right pinky: {}'.format(PoseLandmark.RIGHT_PINKY))
print('left index: {}'.format(PoseLandmark.LEFT_INDEX))
print('right index: {}'.format(PoseLandmark.RIGHT_INDEX))
print('left thumb: {}'.format(PoseLandmark.LEFT_THUMB))
print('right thumb: {}'.format(PoseLandmark.RIGHT_THUMB))
print('left hip: {}'.format(PoseLandmark.LEFT_HIP))
print('right hip: {}'.format(PoseLandmark.RIGHT_HIP))
print('left knee: {}'.format(PoseLandmark.LEFT_KNEE))
print('right knee: {}'.format(PoseLandmark.RIGHT_KNEE))
print('left ankle: {}'.format(PoseLandmark.LEFT_ANKLE))
print('right ankle: {}'.format(PoseLandmark.RIGHT_ANKLE))
print('left heel: {}'.format(PoseLandmark.LEFT_HEEL))
print('right heel: {}'.format(PoseLandmark.RIGHT_HEEL))
print('left foot index: {}'.format(PoseLandmark.LEFT_FOOT_INDEX))
print('right foot index: {}'.format(PoseLandmark.RIGHT_FOOT_INDEX))

from mediapipe.python.solutions.holistic import HandLandmark

index_to_name_hand = [
    'wrist',
    'thumb_cmc', 'thumb_mcp', 'thumb_ip', 'thumb_tip',
    'index_finger_mcp', 'index_finger_pip', 'index_finger_dip', 'index_finger_tip',
    'middle_finger_mcp', 'middle_finger_pip', 'middle_finger_dip', 'middle_finger_tip',
    'ring_finger_mcp', 'ring_finger_pip', 'ring_finger_dip', 'ring_finger_tip',
    'pinky_mcp', 'pinky_pip', 'pinky_dip', 'pinky_tip',
]

print('wrist: {}'.format(HandLandmark.WRIST))
print('thumb cmc: {}'.format(HandLandmark.THUMB_CMC))
print('thumb mcp: {}'.format(HandLandmark.THUMB_MCP))
print('thumb ip: {}'.format(HandLandmark.THUMB_IP))
print('thumb tip: {}'.format(HandLandmark.THUMB_TIP))
print('index mcp: {}'.format(HandLandmark.INDEX_FINGER_MCP))
print('index pip: {}'.format(HandLandmark.INDEX_FINGER_PIP))
print('index dip: {}'.format(HandLandmark.INDEX_FINGER_DIP))
print('index tip: {}'.format(HandLandmark.INDEX_FINGER_TIP))
print('middle mcp: {}'.format(HandLandmark.MIDDLE_FINGER_MCP))
print('middle pip: {}'.format(HandLandmark.MIDDLE_FINGER_PIP))
print('middle dip: {}'.format(HandLandmark.MIDDLE_FINGER_DIP))
print('middle tip: {}'.format(HandLandmark.MIDDLE_FINGER_TIP))
print('ring mcp: {}'.format(HandLandmark.RING_FINGER_MCP))
print('ring pip: {}'.format(HandLandmark.RING_FINGER_PIP))
print('ring dip: {}'.format(HandLandmark.RING_FINGER_DIP))
print('ring tip: {}'.format(HandLandmark.RING_FINGER_TIP))
print('pinky mcp: {}'.format(HandLandmark.PINKY_MCP))
print('pinky pip: {}'.format(HandLandmark.PINKY_PIP))
print('pinky dip: {}'.format(HandLandmark.PINKY_DIP))
print('pinky tip: {}'.format(HandLandmark.PINKY_TIP))
