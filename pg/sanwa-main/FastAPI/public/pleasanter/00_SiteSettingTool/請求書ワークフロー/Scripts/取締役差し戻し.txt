//取締役差し戻し処理
async function DirectorReturn(e){
    try{
        //確認フォーム
        let answer = confirm('この申請を差し戻します。よろしいですか。')

        //キャンセルする場合終了
        if(!answer){
            return
        }
        //ローディング
        $p.loading(e)
        //差し戻した人の名前を取得
        let director = ''
        await $.ajax({
            url: 'http://192.168.10.54/api/users/get',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                "View": {
                    "ColumnFilterHash": {
                        "UserId":$p.userId(),
                    }
                }
            }),
            success: function(response) {
                console.log('成功:', response);
                director  = response.Response.Data[0].Name
                console.log(director)
                
            },
            error: function(xhr, status, error) {
                console.error('エラー:', error);
            }
        });
        //メール用の変数
        let mailTitle = '【請求書】差戻通知'
        let mailBody = [
            `${director}より請求書の申請が差し戻されました。下記URLよりご確認ください。`,
            `${location.origin}/items/${$p.id()}`
        ]
        //ステータス変更と承認者と承認日を空白にする
        await $p.apiUpdate({
            id:$p.id(),
            data: {
                ApiVersion: 1.1,
                Status:100,//申請前
                Manager:'',//承認者を空白
                DateHash:{
                    DateB:'1899/12/31',//承認日を空白
                    DateC:'1899/12/31'
                },
                ClassHash:{
                    ClassB:'',
                    ClassI:'',//処理用承認者
                    ClassJ:'',
                    ClassK:'',
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
            Text: '差し戻しました。'
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
