

import openai

# API キーを直接指定（※セキュリティ上のリスクがあるため、開発環境などでのみ使用してください）
openai.api_key = "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"

response = openai.ChatCompletion.create(
    model="gpt-3.5-turbo",
    messages=[
        {"role": "user", "content": "Hello, who are you? 好きな色は？"}
    ]
)

print(response['choices'][0]['message']['content']) # type: ignore
