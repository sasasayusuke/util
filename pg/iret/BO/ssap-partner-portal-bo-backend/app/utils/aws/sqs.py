from typing import List

import boto3


class SqsHelper:
    def __init__(self):
        self.resource = boto3.resource("sqs")
        self.client = boto3.client("sqs")

    def send_message_to_queue(self, queue_name, message_body):
        queue = self.resource.get_queue_by_name(QueueName=queue_name)
        response = queue.send_message(MessageBody=message_body)

        return response

    def send_message_batch_to_queue(self, queue_name, entries: List[dict]):
        queue = self.resource.get_queue_by_name(QueueName=queue_name)
        n = 10
        # 上限10のため、10個ずつ分割して実行
        for i in range(0, len(entries), n):
            self.client.send_message_batch(
                QueueUrl=queue.url, Entries=entries[i : i + n]
            )
