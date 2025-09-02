//部長承認処理
async function ManagerApproval(e){
    try{
        //確認フォーム
        let answer = confirm('この申請を承認します。よろしいですか。')

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
//----------------------------メール送信先取得処理-----------------------------
        let applicantDept = $p.getControl('ClassH').val() //申請者の組織取得
        let users = ""
        //メール送信先取得処理
        //組織を指定してユーザ情報取得
        await $.ajax({
            url: 'http://192.168.10.54/api/users/get',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                "View": {
                    "ColumnFilterHash": {
                        "DeptId":applicantDept,
                    }
                }
            }),
            success: function(response) {
                console.log('成功:', response);
                users = response.Response.Data
                console.log(users)
                
            },
            error: function(xhr, status, error) {
                console.error('エラー:', error);
            }
        });
        //取得したユーザ情報からユーザIDだけを取り出す
        let userIds = users.map(item => item.UserId)
        console.log(userIds)

        //取締役のグループ情報取得
        let directors = ""
        let directorId = ''
        await $p.apiGroupsGet({
            id:1,
            done: function (data) {
                console.log(data);
                console.log('取締役グループ情報の取得に成功しました。');
                directors = data.Response.Data[0].GroupMembers
                console.log(directors)
                // 取得したグループの中からユーザIDの部分を取り出す
                directors = directors.map(item => {
                    const parts = item.split(','); // カンマで分割
                    return parseInt(parts[1], 10); // 2番目の部分を数値に変換して返す
                });
                //該当の組織に所属している部長のユーザID取得
                directorId = userIds.filter(userId => directors.includes(userId));
                console.log(directorId)
            },
            fail: function () {
                console.log('取締役グループ情報の取得に失敗しました。');
            },
        });
        // // 取得したグループの中からユーザIDの部分を取り出す
        // directors = directors.map(item => {
        //     const parts = item.split(','); // カンマで分割
        //     return parseInt(parts[1], 10); // 2番目の部分を数値に変換して返す
        // });
        // //該当の組織に所属している部長のユーザID取得
        // let directorId = userIds.filter(userId => directors.includes(userId));
        // console.log(directorId)
        

        //部長の名前
        let directorName = commonGetVal('ClassB')
        let mailTitle = '【請求書】承認依頼通知'
        let mailBody = [
            `${directorName}より請求書の承認依頼が届きました。下記URLよりご確認ください。`,
            `${location.origin}/items/${$p.id()}`
        ]




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
        //取締役のユーザIDデータ成形
        let directorIdStr = JSON.stringify(directorId.map(String));
        //レコード更新
        await $p.apiUpdate({
            id:$p.id(),
            data: {
                ApiVersion: 1.1,
                Status:350,//部長承認済
                DescriptionHash:{
                    DescriptionC:$p.getControl('DescriptionC').val(),
                },
                ClassHash: {
                    ClassB:$p.getControl('ClassB').val(),//承認者
                    ClassK:directorIdStr,//承認対象
                },
                DateHash:{
                    DateC:formattedDate,//承認日
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
        // 取締役にメール送信処理
        for(director of directorId){
            await $p.apiCreate({
                id:MAIL_TABLE,
                data: {
                    Title:mailTitle,//件名
                    Body:mailBody.join('\n'),//内容
                    ClassHash: {
                        ClassA:director //宛先
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
