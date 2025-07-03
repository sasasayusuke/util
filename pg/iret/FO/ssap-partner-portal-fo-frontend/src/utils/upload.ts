import AWS from 'aws-sdk'
import { Auth } from 'aws-amplify'
import { format as dateFnsFormat } from 'date-fns'
import { getCurrentDate } from '~/utils/common-functions'

interface IS3KeyTemplate {
  [key: string]: string
}

export interface IS3File {
  name: string
  path: string
}

export interface ISolverFile {
  file: IS3File | File | null
  isSaved: boolean
  index: number
}

export interface IFile extends File {
  uploadDatetime: string
}

export const S3_KEY_TEMPLATE: IS3KeyTemplate = {
  PROJECT_KARTE_DOCUMENTS:
    'project/{projectId}/karte/{karteId}/documents/{date}.{extension}',
  PROJECT_KARTE_DELIVERABLES:
    'project/{projectId}/karte/{karteId}/deliverables/{date}.{extension}',
}

export function getDateStr(): string {
  return dateFnsFormat(getCurrentDate(), 'yyyyMMdd-HHmmssSSS')
}

function generateKey(
  template: string,
  date: string = '',
  fileName: string = '',
  projectId: string = '',
  karteId: string = '',
  extension: string = ''
): string {
  const rtn = template
    .replace('{date}', date)
    .replace('{fileName}', fileName)
    .replace('{projectId}', projectId)
    .replace('{karteId}', karteId)
    .replace('{extension}', extension)

  return rtn
}

export async function uploadFile(
  file: IFile,
  template: string,
  projectId: string = '',
  karteId: string = '',
  extension: string = ''
) {
  const fileName = file.name
  const uploadDatetime = file.uploadDatetime

  const key = generateKey(
    template,
    uploadDatetime,
    fileName,
    projectId,
    karteId,
    extension
  )

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

export const S3_KEY_TEMPLATE_SOLVER_CORPORATION: IS3KeyTemplate = {
  CORPORATION_LOGOS:
    'solver-corporation/{solverCorporationId}/logos/{fileName}_{date}.{extension}',
  CORPORATION_DOCUMENTS:
    'solver-corporation/{solverCorporationId}/documents/{fileName}_{date}.{extension}',
}

export function generateKeySolverCorporation(
  template: string,
  date: string = '',
  fileName: string = '',
  solverCorporationId: string = '',
  extension: string = ''
): string {
  const rtn = template
    .replace('{date}', date)
    .replace('{fileName}', fileName)
    .replace('{solverCorporationId}', solverCorporationId)
    .replace('{extension}', extension)

  return rtn
}

export async function uploadFileSolverCorporation(
  file: IFile,
  template: string,
  solverCorporationId: string = '',
  extension: string = ''
) {
  const fileName = file.name.replace(`.${extension}`, '')
  const uploadDatetime = file.uploadDatetime

  const key = generateKeySolverCorporation(
    template,
    uploadDatetime,
    fileName,
    solverCorporationId,
    extension
  )

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

export const S3_KEY_TEMPLATE_SOLVER: IS3KeyTemplate = {
  SOLVER_FILES:
    'solver-corporation/{solverCorporationId}/solvers/{fileKeyId}/resumes/{fileName}_{date}.{extension}',
  SOLVER_PHOTOS:
    'solver-corporation/{solverCorporationId}/solvers/{fileKeyId}/photos/{fileName}_{date}.{extension}',
}

export function generateKeySolver(
  template: string,
  solverCorporationId: string = '',
  fileKeyId: string = '',
  fileName: string = '',
  date: string = '',
  extension: string = ''
): string {
  const rtn = template
    .replace('{solverCorporationId}', solverCorporationId)
    .replace('{fileKeyId}', fileKeyId)
    .replace('{fileName}', fileName)
    .replace('{date}', date)
    .replace('{extension}', extension)

  return rtn
}

export async function uploadFileSolver(
  template: string,
  file: IFile,
  solverCorporationId: string = '',
  fileKeyId: string = '',
  extension: string = ''
) {
  const fileName = file.name.replace(`.${extension}`, '')
  const uploadDatetime = file.uploadDatetime

  const key = generateKeySolver(
    template,
    solverCorporationId,
    fileKeyId,
    fileName,
    uploadDatetime,
    extension
  )

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
