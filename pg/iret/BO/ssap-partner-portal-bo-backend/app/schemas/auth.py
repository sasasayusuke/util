from pydantic import BaseModel, Field


class VerifyOTPRequest(BaseModel):
    one_time_password: str = Field(..., description="OTP", example="99999999")


class VerifyOTPResponse(BaseModel):
    user_id: str = Field(
        ..., description="ユーザーID", example='"89cbe2ed-f44c-4a1c-9408-c67b0ca2270d'
    )
    otp_verified_token: str = Field(
        ..., description="OTP検証済みトークン", example="KDjNROK0gMO9ztvabecJCg"
    )
