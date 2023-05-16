from fastapi import FastAPI
from pydantic import BaseModel

from .ai import mediapipe_solutions as mps

class AIData(BaseModel):
    img: str
    ts: int
    w: int
    h: int

class Data(BaseModel):
    accessToken: str
    data: AIData

class ConnectionData(BaseModel):
    clientName: str

app = FastAPI()

@app.post("/connect")
async def connect(data: ConnectionData = None):
    token = mps.connect(data.clientName)
    return { "success": True, "token": token }

@app.post("/disconnect")
async def disconnect(data: ConnectionData = None):
    mps.disconnect(data.clientName)
    return { "success": True }

@app.post("/process")
async def processData(data: Data = None):
    return mps.process_image(data.accessToken, data.data.img, data.data.ts, data.data.w, data.data.h)