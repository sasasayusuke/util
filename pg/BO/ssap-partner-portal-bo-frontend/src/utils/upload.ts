import AWS from 'aws-sdk'
import { Auth } from 'aws-amplify'
import { format as dateFnsFormat } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'

interface IS3KeyTemplate {
  [key: string]: string
}

export const S3_KEY_TEMPLATE: IS3KeyTemplate = {
  IMPORT_CUSTOMER: 'import/customer/{date}/{fileName}',
  IMPORT_PROJECT: 'import/project/{date}/{fileName}',
}

function getDateStr(): string {
  return dateFnsFormat(getCurrentDate(), 'yyyyMMdd-HHmmss')
}

function generateKey(
  template: string,
  fileName: string = '',
  projectId: string = '',
  karteId: string = '',
  extension: string = ''
): string {
  const rtn = template
    .replace('{date}', getDateStr())
    .replace('{fileName}', fileName)
    .replace('{projectId}', projectId)
    .replace('{karteId}', karteId)
    .replace('{extension}', extension)

  return rtn
}

export async function uploadFile(
  file: File,
  template: string,
  projectId: string = '',
  karteId: string = '',
  extension: string = ''
) {
  const fileName = file.name

  const key = generateKey(template, fileName, projectId, karteId, extension)

  // リージョン指定
  AWS.config = new AWS.Config({
    region: process.env.S3_BUCKET_REGION,
    computeChecksums: true,
  })
  // credentialsを取得し、ファイルをアップロード
  return await Auth.currentCredentials().then((credentials) => {
    AWS.config.update({
      credentials,
    })

    const upload = new AWS.S3.ManagedUpload({
      // アップロード先を指定
      params: {
        Bucket: `${process.env.S3_UPLOAD_BUCKET_NAME}`,
        Key: key,
        Body: file,
        ContentType: file.type,
      },
    })
    return upload.promise()
  })
}
