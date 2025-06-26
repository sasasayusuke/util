import os
import boto3


class S3Helper:
    def __init__(self):
        self.resource = boto3.resource("s3")

    def get_object(self, bucket_name, object_key):
        return self.resource.Object(bucket_name, object_key)

    def get_object_content(self, bucket_name, object_key, encoding="utf-8"):
        object = self.get_object(bucket_name=bucket_name, object_key=object_key)

        body = object.get()["Body"].read()

        # ファイル拡張子がtxtの場合のみ、utf-8でデコード
        ext = os.path.splitext(object_key)
        if ext[1].lower() == ".txt":
            return body.decode(encoding)
        else:
            return body
