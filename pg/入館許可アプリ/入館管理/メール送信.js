/**
 * メール送信関数
 *
 * @param {Object} options - メール送信オプション
 * @param {string} options.to - 宛先メールアドレス（必須）
 * @param {string} options.subject - 件名（必須）
 * @param {string} options.body - 本文（必須）
 * @param {boolean} [options.includeAttachments=false] - 添付ファイルを含めるかどうか（デフォルト: false）
 * @param {string} [options.attachmentField='AttachmentsA'] - 添付ファイルを取得する項目名（デフォルト: 'AttachmentsA'）
 * @param {string} [options.attachmentRecordId] - 添付ファイルを取得するレコードID（指定しない場合は現在のレコード）
 * @param {string} [options.cc] - CCメールアドレス
 * @param {string} [options.bcc] - BCCメールアドレス
 *
 * @description
 * この関数は現在のレコードに基づいてメールを送信します。
 * デフォルトでは添付ファイルなしでメールを送信します。
 * 添付ファイルを含める場合は、includeAttachmentsオプションをtrueに設定してください。
 *
 * @example
 * // 基本的なメール送信（添付ファイルなし）
 * sendMail({
 *   to: 'recipient@example.com',
 *   subject: '件名',
 *   body: 'メール本文'
 * });
 *
 * @example
 * // 添付ファイル付きでメール送信
 * sendMail({
 *   to: 'recipient@example.com',
 *   subject: '件名',
 *   body: 'メール本文',
 *   includeAttachments: true
 * });
 *
 * @example
 * // 別のレコードから添付ファイルを取得してメール送信
 * sendMail({
 *   to: 'recipient@example.com',
 *   subject: '件名',
 *   body: 'メール本文',
 *   includeAttachments: true,
 *   attachmentRecordId: 12345
 * });
 *
 * @example
 * // カスタム設定でメール送信
 * sendMail({
 *   to: 'custom@example.com',
 *   cc: 'cc@example.com',
 *   subject: 'カスタム件名',
 *   body: 'メール本文',
 *   includeAttachments: true
 * });
 */
function sendMail(options = {}) {
	// 必須パラメータの検証
	if (!options.to) {
		console.error('エラー: 宛先メールアドレス(to)が未指定です');
		alert('宛先メールアドレス(to)は必須です');
		return;
	}
	if (!options.subject) {
		console.error('エラー: 件名(subject)が未指定です');
		alert('件名(subject)は必須です');
		return;
	}
	if (!options.body) {
		console.error('エラー: 本文(body)が未指定です');
		alert('本文(body)は必須です');
		return;
	}

	// オプションのデフォルト値を設定
	const config = {
		includeAttachments: options.includeAttachments === true, // デフォルトはfalse
		attachmentField: options.attachmentField || 'AttachmentsA',
		attachmentRecordId: options.attachmentRecordId || $p.id(), // デフォルトは現在のレコード
		to: options.to,
		cc: options.cc || "",
		bcc: options.bcc || "",
		subject: options.subject,
		body: options.body
	};

	// メール設定
	let to = config.to;
	let cc = config.cc;
	let bcc = config.bcc;
	let title = config.subject;
	let body = config.body;

	console.log('sendMail関数内部 - パラメータ確認:');
	console.log('- to:', to);
	console.log('- cc:', cc);
	console.log('- bcc:', bcc);
	console.log('- title:', title);
	console.log('- body:', body ? body.substring(0, 100) + '...' : 'undefined');
	const errorGetMail = 'メールデータ取得に失敗しました';
	const errorSendMail = 'メール送信処理に失敗しました';

	let postAttachmentsHash = [];
	let attachments = [];
	let id = config.attachmentRecordId; // 設定されたレコードIDを使用
	let apiSendMailUrl = $p.apiSendMailUrl($p.id()); // メール送信は現在のレコードから

	let isOkResponseData = function (data) {
		let isOk = true;
		if (data == null || data == undefined
			|| data.Response == null || data.Response == undefined
			|| data.Response.Data[0] == null || data.Response.Data[0] == undefined) {
			isOk = false;
		}
		return isOk;
	}

	let isEmpty = function (str) {
		let isEmpty = false;
		if (str == null || str == undefined || str.trim().length <= 0) {
			isEmpty = true;
		}
		return isEmpty;
	}

	// 添付ファイルが必要な場合のみ$p.apiGetを実行
	if (config.includeAttachments) {
		$p.apiGet({
			'id': id,
			'done': function (data) {
				if (!isOkResponseData(data)) {
					alert(errorGetMail);
					return;
				}
				// メールに添付する添付ファイルを格納する項目を定義
				let attachmentList = [];

				// 指定された項目から添付ファイルを取得（デフォルト: AttachmentsA）
				const attachmentsHash = data.Response.Data[0].AttachmentsHash || {};
				attachmentList = attachmentsHash[config.attachmentField] || [];

				let createAttachments = async function () {
					if (attachmentList.length == 0) {
						return Promise.resolve();
					}

					const promises = attachmentList.map((attachment, index) => {
						return new Promise((resolve, reject) => {
							postAttachmentsHash[index] = {
								'Guid': attachment.Guid,
								'Deleted': 1,
							};

							// 下の変数urlのパスは環境によって異なりますので適宜変更してください。
							let url = '/binaries/' + attachment.Guid + '/download';
							let xhr = new XMLHttpRequest();
							xhr.open('GET', url, true);
							xhr.setRequestHeader('Content-Type', 'application/json');
							xhr.responseType = 'arraybuffer';

							xhr.onload = function (oEvent) {
								if (xhr.readyState == 4 && xhr.status == 200) {
									let bytes = new Uint8Array(xhr.response);
									let binaryString = '';
									for (let j = 0; j < bytes.byteLength; j++) {
										binaryString += String.fromCharCode(bytes[j]);
									}
									let base64 = window.btoa(binaryString);
									attachments[index] = {
										Name: attachment.Name,
										Base64: base64,
										ContentType: xhr.getResponseHeader('content-type')
									};
									resolve();
								} else {
									reject(new Error('Failed to load attachment'));
								}
							};

							xhr.onerror = function () {
								reject(new Error('Network error'));
							};

							xhr.send();
						});
					});

					return Promise.all(promises);
				};

				createAttachments().then(function (value) {
					let postData = {
						To: to,
						Cc: cc,
						Bcc: bcc,
						Title: isEmpty(title) ? ' ' : title,
						Body: isEmpty(body) ? ' ' : body,
						Attachments: attachments
					};
					console.log(postData);

					$.ajax({
						url: apiSendMailUrl,
						type: 'post',
						async: false,
						cache: false,
						data: JSON.stringify(postData),
						contentType: 'application/json',
						dataType: 'json'
					})
						.done(function (json, textStatus, jqXHR) {
							console.log('メール送信成功');
						})
						.fail(function (jqXHR, textStatus, errorThrown) {
							alert(errorSendMail);
						})
						.always(function () {
							console.log('メール送信処理完了');
						});
				}).catch(function (error) {
					console.error('添付ファイル処理エラー:', error);
					alert('添付ファイルの処理中にエラーが発生しました');
				});
			},
			'fail': function (data) {
				alert(errorGetMail);
			},
			'always': function (data) {
			},
			'async': false
		});
	} else {
		// 添付ファイルがない場合は直接メール送信
		let postData = {
			To: to,
			Cc: cc,
			Bcc: bcc,
			Title: isEmpty(title) ? ' ' : title,
			Body: isEmpty(body) ? ' ' : body,
			Attachments: []
		};
		console.log(postData);

		$.ajax({
			url: apiSendMailUrl,
			type: 'post',
			async: false,
			cache: false,
			data: JSON.stringify(postData),
			contentType: 'application/json',
			dataType: 'json'
		})
			.done(function (json, textStatus, jqXHR) {
				console.log('メール送信成功');
			})
			.fail(function (jqXHR, textStatus, errorThrown) {
				alert(errorSendMail);
			})
			.always(function () {
				console.log('メール送信処理完了');
			});
	}
}