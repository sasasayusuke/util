//取締役承認処理
async function DirectorApproval(e){
    try{
        //確認フォーム
        let answer = confirm('この申請を承認します。よろしいですか。')

        //キャンセルする場合終了
        if(!answer){
            return
        }
        //承認者が空白だったらエラー
        let approver = $p.getControl('ClassF').val()
        if(!approver){
            $p.setMessage('#Message', JSON.stringify({
                Css: 'alert-error',
                Text: '承認者を選択してください。'
            }));
            return
        }
        //ローディング
        $p.loading(e)
//----------------------------メール送信先取得処理-----------------------------
        //メール用の変数
        let presidentId = '' //社長のユーザID
        //取締役の名前
        let directorName = commonGetVal('ClassF')
        let mailTitle = '【請求書】承認依頼通知'
        let mailBody = [
            `${directorName}より請求書の承認依頼が届きました。下記URLよりご確認ください。`,
            `${location.origin}/items/${$p.id()}`
        ]

        //ユーザ情報からメールアドレスを取得
        await $.ajax({
        url: 'http://192.168.10.54/api/users/get',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            "View": {
            "ApiGetMailAddresses": true,
            "ColumnFilterHash": {
                "DeptId":1,//三和商研代表
            }
        }
        }),
        success: function(response) {
            console.log('成功:', response);
            presidentId = response.Response.Data
            //取得したユーザ情報からメールアドレスだけを取り出す
            presidentId = presidentId.map(item => item.UserId)
            console.log(presidentId)
            
        },
        error: function(xhr, status, error) {
            console.error('エラー:', error);
        }
        });


//------------------------------------------------------------------------------------
        //承認日
        const now = new Date();

        const year = now.getFullYear();
        const month = String(now.getMonth() + 1).padStart(2, '0'); // 月は0から始まるため+1
        const day = String(now.getDate()).padStart(2, '0');
        const hours = String(now.getHours()).padStart(2, '0');
        const minutes = String(now.getMinutes()).padStart(2, '0');
        const seconds = String(now.getSeconds()).padStart(2, '0');
        const formattedDate = `${year}/${month}/${day} ${hours}:${minutes}:${seconds}`;
        //データ成形
        let presidentIdStr = JSON.stringify(presidentId.map(String));
        //レコード更新
        await $p.apiUpdate({
            id:$p.id(),
            data: {
                ApiVersion: 1.1,
                Status:400,//取締役承認済
                DescriptionHash:{
                    DescriptionD:$p.getControl('DescriptionD').val(),
                },
                ClassHash: {
                    ClassF:$p.getControl('ClassF').val(),//承認者
                    ClassL:presidentIdStr,//承認対象
                },
                DateHash:{
                    DateE:formattedDate,//承認日
                }
            },
            done: function (data) {
                console.log('データの更新に成功しました。');
            },
            fail: function (data) {
                console.log('データの更新に失敗しました。');
            },
        });

//------------------------------------------------------------------------------------
        // 社長にメール送信処理
        for(president of presidentId){
            await $p.apiCreate({
                id:MAIL_TABLE,
                data: {
                    Title:mailTitle,//件名
                    Body:mailBody.join('\n'),//内容
                    ClassHash: {
                        ClassA:president //宛先
                    }
                },
                done: function (data) {
                    console.log('メール送信に成功しました。');
                },
                fail: function (data) {
                    console.log(data);
                }
            }); 
        }
        $p.loaded()
        $p.clearMessage();
        $p.setMessage('#Message', JSON.stringify({
            Css: 'alert-success',
            Text: '承認しました。'
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
