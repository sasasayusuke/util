//状況によって読み取り専用にする・申請者と承認対象が自分のレコードのみ更新ボタン表示
$p.events.on_editor_load = function () {
    $("#UpdateCommand").hide()//更新ボタン
    $("#Process_10").hide()//申請ボタン
    let Status = $p.getControl('Status').val()
    let applicant = $p.getControl('Owner').val() //申請者
    let groupManager = $p.getControl('ClassI').val()//グループマネージャー
    let buchou = $p.getControl('ClassJ').val() //部長
    let torishimari = $p.getControl('ClassK').val() //取締役
    let shachou = $p.getControl('ClassL').val()  //社長
    //申請前
    if(Status == 100 ){
        //申請者が自分のときのみ申請者入力エリア編集可
        if(applicant != $p.userId()){
            //申請者入力エリア
            $('#Results_ClassH').prop('disabled', true)//所属部署
            $('#Results_ClassA').prop('disabled', true)//申請区分
            $('#Results_DateD').prop('disabled', true)//起票日時
            $('#Results_ClassC').prop('disabled', true)//請求担当部署
            $('#Results_ClassD').prop('disabled', true)///請求担当者名
            $('#Results_ClassE').prop('disabled', true)//請求処理担当
            $('#Results_DescriptionA').prop('disabled', true)//備考
            $('#AttachmentsA\\.upload').hide();//申請ファイル
            $('.ui-icon.ui-icon-circle-close.delete-file').hide();//ファイル削除アイコン
            $('#Results_Status').prop('disabled', true)//状況
            $('#Results_Owner').prop('disabled', true)//申請者
            //グループマネージャー承認時入力エリア
            $('#Results_Manager').prop('disabled', true)//承認者
            $('#Results_DateB').prop('disabled', true)//承認/棄却日時
            $('#Results_DescriptionB').prop('disabled', true)//コメント①
            //部長承認時入力エリア
            $('#Results_ClassB').prop('disabled', true)//承認者
            $('#Results_DateC').prop('disabled', true)//承認/棄却日時
            $('#Results_DescriptionC').prop('disabled', true)//コメント①
            //取締役承認時入力エリア
            $('#Results_ClassF').prop('disabled', true)//承認者
            $('#Results_DateE').prop('disabled', true)//承認/棄却日時
            $('#Results_DescriptionD').prop('disabled', true)//コメント①
            //最終承認時入力エリア
            $('#Results_ClassG').prop('disabled', true)//承認者
            $('#Results_DateF').prop('disabled', true)//承認/棄却日時
            $('#Results_DescriptionE').prop('disabled', true)//コメント①
            //時計アイコン
            $('.ui-icon.ui-icon-clock.current-time').hide();
            //人物アイコン
            $('.ui-icon.ui-icon-person.current-user').hide();
            $('.ui-icon.ui-icon-person.current-dept').hide();
            
        }else{
            $("#UpdateCommand").show()
            $("#Process_10").show()//申請ボタン
            $('#Results_Status').prop('disabled', true)//状況
            $('#Results_Owner').prop('disabled', true)//申請者
            $('#Results_ClassH').prop('disabled', true)//所属部署
            //グループマネージャー承認時入力エリア
            $('#Results_Manager').prop('disabled', true)//承認者
            $('#Results_DateB').prop('disabled', true)//承認/棄却日時
            $('#Results_DescriptionB').prop('disabled', true)//コメント①
            //部長承認時入力エリア
            $('#Results_ClassB').prop('disabled', true)//承認者
            $('#Results_DateC').prop('disabled', true)//承認/棄却日時
            $('#Results_DescriptionC').prop('disabled', true)//コメント①
            //取締役承認時入力エリア
            $('#Results_ClassF').prop('disabled', true)//承認者
            $('#Results_DateE').prop('disabled', true)//承認/棄却日時
            $('#Results_DescriptionD').prop('disabled', true)//コメント①
            //最終承認時入力エリア
            $('#Results_ClassG').prop('disabled', true)//承認者
            $('#Results_DateF').prop('disabled', true)//承認/棄却日時
            $('#Results_DescriptionE').prop('disabled', true)//コメント①
            //時計アイコン
            $('.ui-icon.ui-icon-clock.current-time').hide();
            //人物アイコン
            $('.ui-icon.ui-icon-person.current-user').hide();
            $('.ui-icon.ui-icon-person.current-dept').hide();
        }
    }
    
    //申請済み
    if(Status == 200 ){
        $('#Results_Status').prop('disabled', true)//状況
        $('#Results_Title').prop('disabled', true)//タイトル
        $('#Results_Owner').prop('disabled', true)//申請者
        $('#Results_ClassH').prop('disabled', true)//所属部署
        $('#Results_ClassA').prop('disabled', true)//申請区分
        $('#Results_DateD').prop('disabled', true)//起票日時
        $('#Results_ClassC').prop('disabled', true)//請求担当部署
        $('#Results_ClassD').prop('disabled', true)///請求担当者名
        $('#Results_ClassE').prop('disabled', true)//請求処理担当
        $('#Results_DescriptionA').prop('disabled', true)//備考
        $('#AttachmentsA\\.upload').hide();//申請ファイル
        $('.ui-icon.ui-icon-circle-close.delete-file').hide();//ファイル削除アイコン
        //承認対象のグループマネージャーのみ編集可能
        if(!groupManager.includes(String($p.userId()))){
            //グループマネージャー承認時入力エリア
            $('#Results_Manager').prop('disabled', true)//承認者
            $('#Results_DescriptionB').prop('disabled', true)//コメント①
        }
        $('#Results_DateB').prop('disabled', true)//承認/棄却日時
        //部長承認時入力エリア
        $('#Results_ClassB').prop('disabled', true)//承認者
        $('#Results_DateC').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionC').prop('disabled', true)//コメント①
        //取締役承認時入力エリア
        $('#Results_ClassF').prop('disabled', true)//承認者
        $('#Results_DateE').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionD').prop('disabled', true)//コメント①
        //最終承認時入力エリア
        $('#Results_ClassG').prop('disabled', true)//承認者
        $('#Results_DateF').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionE').prop('disabled', true)//コメント①
        //削除ボタン
        $('#DeleteCommand').hide()
        //時計アイコン
        $('.ui-icon.ui-icon-clock.current-time').hide();
        //人物アイコン
        $('.ui-icon.ui-icon-person.current-user').hide();
        $('.ui-icon.ui-icon-person.current-dept').hide();
    }
    //グループマネージャー承認
    if(Status == 300){
        //申請時入力エリア
        $('#Results_Status').prop('disabled', true)//状況
        $('#Results_Title').prop('disabled', true)//タイトル
        $('#Results_Owner').prop('disabled', true)//申請者
        $('#Results_ClassH').prop('disabled', true)//所属部署
        $('#Results_ClassA').prop('disabled', true)//申請区分
        $('#Results_DateD').prop('disabled', true)//起票日時
        $('#Results_ClassC').prop('disabled', true)//請求担当部署
        $('#Results_ClassD').prop('disabled', true)///請求担当者名
        $('#Results_ClassE').prop('disabled', true)//請求処理担当
        $('#Results_DescriptionA').prop('disabled', true)//備考
        $('#AttachmentsA\\.upload').hide();//申請ファイル
        $('.ui-icon.ui-icon-circle-close.delete-file').hide();//ファイル削除アイコン
        //グループマネージャー承認時入力エリア
        $('#Results_Manager').prop('disabled', true)//承認者
        $('#Results_DateB').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionB').prop('disabled', true)//コメント①
        //承認対象の部長のみ編集可能
        if(!buchou.includes(String($p.userId()))){
            //部長承認時入力エリア
            $('#Results_ClassB').prop('disabled', true)//承認者
            $('#Results_DescriptionC').prop('disabled', true)//コメント②
        }
        $('#Results_DateC').prop('disabled', true)//承認/棄却日時
        //取締役承認時入力エリア
        $('#Results_ClassF').prop('disabled', true)//承認者
        $('#Results_DateE').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionD').prop('disabled', true)//コメント①
        //最終承認時入力エリア
        $('#Results_ClassG').prop('disabled', true)//承認者
        $('#Results_DateF').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionE').prop('disabled', true)//コメント①
        //削除ボタン
        $('#DeleteCommand').hide()
        //時計アイコン
        $('.ui-icon.ui-icon-clock.current-time').hide();
        //人物アイコン
        $('.ui-icon.ui-icon-person.current-user').hide();
        $('.ui-icon.ui-icon-person.current-dept').hide();
    }
    //部長承認
    if(Status == 350){
        //申請時入力エリア
        $('#Results_Status').prop('disabled', true)//状況
        $('#Results_Title').prop('disabled', true)//タイトル
        $('#Results_Owner').prop('disabled', true)//申請者
        $('#Results_ClassH').prop('disabled', true)//所属部署
        $('#Results_ClassA').prop('disabled', true)//申請区分
        $('#Results_DateD').prop('disabled', true)//起票日時
        $('#Results_ClassC').prop('disabled', true)//請求担当部署
        $('#Results_ClassD').prop('disabled', true)///請求担当者名
        $('#Results_ClassE').prop('disabled', true)//請求処理担当
        $('#Results_DescriptionA').prop('disabled', true)//備考
        $('#AttachmentsA\\.upload').hide();//申請ファイル
        $('.ui-icon.ui-icon-circle-close.delete-file').hide();//ファイル削除アイコン
        //グループマネージャー承認時入力エリア
        $('#Results_Manager').prop('disabled', true)//承認者
        $('#Results_DateB').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionB').prop('disabled', true)//コメント①
        //部長承認時入力エリア
        $('#Results_ClassB').prop('disabled', true)//承認者
        $('#Results_DateC').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionC').prop('disabled', true)//コメント①
        //承認対象の取締役のみ編集可能
        if(!torishimari.includes(String($p.userId()))){
            //取締役承認時入力エリア
            $('#Results_ClassF').prop('disabled', true)//承認者
            $('#Results_DescriptionD').prop('disabled', true)//コメント③
        }
        $('#Results_DateE').prop('disabled', true)//承認/棄却日時
        //最終承認時入力エリア
        $('#Results_ClassG').prop('disabled', true)//承認者
        $('#Results_DateF').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionE').prop('disabled', true)//コメント①
        //削除ボタン
        $('#DeleteCommand').hide() 
        //時計アイコン
        $('.ui-icon.ui-icon-clock.current-time').hide();
        //人物アイコン
        $('.ui-icon.ui-icon-person.current-user').hide();
        $('.ui-icon.ui-icon-person.current-dept').hide();
    }
    //取締役承認
    if(Status == 400){
        //申請時入力エリア
        $('#Results_Status').prop('disabled', true)//状況
        $('#Results_Title').prop('disabled', true)//タイトル
        $('#Results_Owner').prop('disabled', true)//申請者
        $('#Results_ClassH').prop('disabled', true)//所属部署
        $('#Results_ClassA').prop('disabled', true)//申請区分
        $('#Results_DateD').prop('disabled', true)//起票日時
        $('#Results_ClassC').prop('disabled', true)//請求担当部署
        $('#Results_ClassD').prop('disabled', true)///請求担当者名
        $('#Results_ClassE').prop('disabled', true)//請求処理担当
        $('#Results_DescriptionA').prop('disabled', true)//備考
        $('#AttachmentsA\\.upload').hide();//申請ファイル
        $('.ui-icon.ui-icon-circle-close.delete-file').hide();//ファイル削除アイコン
        //グループマネージャー承認時入力エリア
        $('#Results_Manager').prop('disabled', true)//承認者
        $('#Results_DateB').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionB').prop('disabled', true)//コメント①
        //部長承認時入力エリア
        $('#Results_ClassB').prop('disabled', true)//承認者
        $('#Results_DateC').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionC').prop('disabled', true)//コメント①
        //取締役承認時入力エリア
        $('#Results_ClassF').prop('disabled', true)//承認者
        $('#Results_DateE').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionD').prop('disabled', true)//コメント①
        //承認対象の社長のみ編集可能
        if(!shachou.includes(String($p.userId()))){
            //最終承認時入力エリア
            $('#Results_ClassG').prop('disabled', true)//承認者
            $('#Results_DescriptionE').prop('disabled', true)//コメント④
        }
        $('#Results_DateF').prop('disabled', true)//承認/棄却日時
        //削除ボタン
        $('#DeleteCommand').hide()
        //時計アイコン
        $('.ui-icon.ui-icon-clock.current-time').hide();
        //人物アイコン
        $('.ui-icon.ui-icon-person.current-user').hide();
        $('.ui-icon.ui-icon-person.current-dept').hide();
    }
    //最終承認
    if(Status == 500){
        //申請時入力エリア
        $('#Results_Status').prop('disabled', true)//状況
        $('#Results_Title').prop('disabled', true)//タイトル
        $('#Results_Owner').prop('disabled', true)//申請者
        $('#Results_ClassH').prop('disabled', true)//所属部署
       $('#Results_ClassA').prop('disabled', true)//申請区分
        $('#Results_DateD').prop('disabled', true)//起票日時
        $('#Results_ClassC').prop('disabled', true)//請求担当部署
        $('#Results_ClassD').prop('disabled', true)///請求担当者名
        $('#Results_ClassE').prop('disabled', true)//請求処理担当
        $('#Results_DescriptionA').prop('disabled', true)//備考
        $('#AttachmentsA\\.upload').hide();//申請ファイル
        $('.ui-icon.ui-icon-circle-close.delete-file').hide();//ファイル削除アイコン
        //グループマネージャー承認時入力エリア
        $('#Results_Manager').prop('disabled', true)//承認者
        $('#Results_DateB').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionB').prop('disabled', true)//コメント①
        //部長承認時入力エリア
        $('#Results_ClassB').prop('disabled', true)//承認者
        $('#Results_DateC').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionC').prop('disabled', true)//コメント①
        //取締役承認時入力エリア
        $('#Results_ClassF').prop('disabled', true)//承認者
        $('#Results_DateE').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionD').prop('disabled', true)//コメント①
        //最終承認時入力エリア
        $('#Results_ClassG').prop('disabled', true)//承認者
        $('#Results_DateF').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionE').prop('disabled', true)//コメント①
        //削除ボタン
        $('#DeleteCommand').hide()
        //時計アイコン
        $('.ui-icon.ui-icon-clock.current-time').hide();
        //人物アイコン
        $('.ui-icon.ui-icon-person.current-user').hide();
        $('.ui-icon.ui-icon-person.current-dept').hide();

    }
    //棄却
    if(Status == 600){
        //申請時入力エリア
        $('#Results_Status').prop('disabled', true)//状況
        $('#Results_Title').prop('disabled', true)//タイトル
        $('#Results_Owner').prop('disabled', true)//申請者
        $('#Results_ClassH').prop('disabled', true)//所属部署
        $('#Results_ClassA').prop('disabled', true)//申請区分
        $('#Results_DateD').prop('disabled', true)//起票日時
        $('#Results_ClassC').prop('disabled', true)//請求担当部署
        $('#Results_ClassD').prop('disabled', true)///請求担当者名
        $('#Results_ClassE').prop('disabled', true)//請求処理担当
        $('#Results_DescriptionA').prop('disabled', true)//備考
        $('#AttachmentsA\\.upload').hide();//申請ファイル
        $('.ui-icon.ui-icon-circle-close.delete-file').hide();//ファイル削除アイコン
        //グループマネージャー承認時入力エリア
        $('#Results_Manager').prop('disabled', true)//承認者
        $('#Results_DateB').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionB').prop('disabled', true)//コメント①
        //部長承認時入力エリア
        $('#Results_ClassB').prop('disabled', true)//承認者
        $('#Results_DateC').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionC').prop('disabled', true)//コメント①
        //取締役承認時入力エリア
        $('#Results_ClassF').prop('disabled', true)//承認者
        $('#Results_DateE').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionD').prop('disabled', true)//コメント①
        //最終承認時入力エリア
        $('#Results_ClassG').prop('disabled', true)//承認者
        $('#Results_DateF').prop('disabled', true)//承認/棄却日時
        $('#Results_DescriptionE').prop('disabled', true)//コメント①
        //削除ボタン
        $('#DeleteCommand').hide()
        //時計アイコン
        $('.ui-icon.ui-icon-clock.current-time').hide();
        //人物アイコン
        $('.ui-icon.ui-icon-person.current-user').hide();
        $('.ui-icon.ui-icon-person.current-dept').hide();

    }

}