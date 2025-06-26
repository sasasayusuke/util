import AWS from 'aws-sdk'
import { Auth } from 'aws-amplify'

export async function deleteFile(key: string) {
  // リージョン指定
  AWS.config = new AWS.Config({
    region: process.env.S3_BUCKET_REGION,
  })

  // credentialsを取得し、ファイルを削除
  return await Auth.currentCredentials().then((credentials) => {
    AWS.config.update({
      credentials,
    })
    const s3 = new AWS.S3()
    const clear = s3.deleteObject({
      Bucket: `${process.env.S3_UPLOAD_BUCKET_NAME}`,
      Key: key,
    })

    return clear.promise()
  })
}
