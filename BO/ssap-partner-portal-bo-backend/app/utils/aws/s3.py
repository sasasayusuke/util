import boto3


class S3Helper:
    def __init__(self):
        self.resource = boto3.resource("s3")
        self.client = boto3.client("s3")

    def get_object(self, bucket_name, object_key):
        return self.resource.Object(bucket_name, object_key)

    def get_object_content(self, bucket_name, object_key, encoding="utf-8"):
        object = self.get_object(bucket_name=bucket_name, object_key=object_key)

        return object.get()["Body"].read().decode(encoding)

    def put_object(self, upload_file_name, bucket_name, object_key):
        return self.client.upload_file(upload_file_name, bucket_name, object_key)

    def generate_presigned_url(self, method, bucket_name, object_key, ttl):
        return self.client.generate_presigned_url(
            ClientMethod=method,
            Params={"Bucket": bucket_name, "Key": object_key},
            ExpiresIn=ttl,
        )
