//部長棄却処理
async function ManagerReject(e){
    try{
        //確認フォーム
        let answer = confirm('この申請を却下します。よろしいですか。')

        //キャンセルする場合終了
        if(!answer){
            return
        }
        //承認者が空白だったらエラー
        let approver = $p.getControl('ClassB').val()
        if(!approver){
            $p.setMessage('#Message', JSON.stringify({
                Css: 'alert-error',
                Text: '承認者を選択してください。'
            }));
            return
        }
        //ローディング
        $p.loading(e)

        //メール用の変数
        let Manager = commonGetVal('ClassB')
        let mailTitle = '【請求書】申請棄却通知'
        let mailBody = [
            `${Manager}より請求書の申請が却下されました。下記URLよりご確認ください。`,
            `${location.origin}/items/${$p.id()}`
        ]

        //承認日
        const now = new Date();

        const year = now.getFullYear();
        const month = String(now.getMonth() + 1).padStart(2, '0'); // 月は0から始まるため+1
        const day = String(now.getDate()).padStart(2, '0');
        const hours = String(now.getHours()).padStart(2, '0');
        const minutes = String(now.getMinutes()).padStart(2, '0');
        const seconds = String(now.getSeconds()).padStart(2, '0');
        const formattedDate = `${year}/${month}/${day} ${hours}:${minutes}:${seconds}`;
        //レコード更新
        await $p.apiUpdate({
            id:$p.id(),
            data: {
                ApiVersion: 1.1,
                Status:600,//棄却
                DescriptionHash:{
                    DescriptionC:$p.getControl('DescriptionC').val()
                },
                DateHash:{
                    DateC:formattedDate,//棄却日
                },
                ClassHash:{
                    ClassB:$p.getControl('ClassB').val(),//承認者
                }
            },
            done: function (data) {
                console.log('データの更新に成功しました。');
            },
            fail: function (data) {
                console.log('データの更新に失敗しました。');
            },
        });

        //申請者にメール送信
        let applicant = $p.getControl('Owner').val() //申請者
        await $p.apiCreate({
            id:MAIL_TABLE,
            data: {
                Title:mailTitle,//件名
                Body:mailBody.join('\n'),//内容
                ClassHash: {
                    ClassA:applicant //宛先
                }
            },
            done: function (data) {
                console.log('メール送信に成功しました。');
            },
            fail: function (data) {
                console.log(data);
            }
        }); 
        
        $p.loaded()
        $p.clearMessage();         
        $p.setMessage('#Message', JSON.stringify({
            Css: 'alert-success',
            Text: '却下しました。'
        }));

    }catch(e){
        if(e.message.startsWith('E01')||e.message.startsWith('E02')||e.message.startsWith('E03')||e.message.startsWith('E04')||e.message.startsWith('E05')||e.message.startsWith('E06')){
            $p.setMessage('#Message', JSON.stringify({
                Css: 'alert-error',
                Text: e.message
            }));
            console.log(e.message)
        }else{
            $p.setMessage('#Message', JSON.stringify({
                Css: 'alert-error',
                Text: 'E99:エラーが発生しました。サポートにお問い合わせください。'
            }));
            console.log(e.message)
        }
        $p.loaded()
    }
}
