$('#Results_ClassA').change(function(){
        if($('#Results_ClassA').val()=='貸出可否' ){
            $('#Results_DescriptionA').prop('disabled', false);
            $('#Results_DescriptionA').css('background-color','#ffffff');
        }else{
　　　　$p.set($('#Results_DescriptionA'),'');
            $('#Results_DescriptionA').prop('disabled', true);
            $('#Results_DescriptionA').css('background-color','#dddddd');
        }
});