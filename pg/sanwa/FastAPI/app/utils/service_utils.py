import datetime
from app.core.exceptions import ServiceError
import logging
from datetime import datetime
from app.core.config import settings
from app.utils.db_utils import SQLExecutor
from app.utils.string_utils import null_to_zero

# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

class ModKubuns():
    def GetBukkenSyubetumei(bukken_no):
        status_dict = {
            0: "新店",
            1: "改装",
            2: "ﾒﾝﾃ",
            3: "補充",
            4: "内装",
            5: "その他",
            6: "委託"
        }
        return status_dict.get(bukken_no, "")

    def GetSagyouKubunmei(sagyou_no):
        status_dict = {
            1: "工事",
            2: "コール",
            3: "内装",
            4: "ｸﾚｰﾑ"
        }
        return status_dict.get(sagyou_no, "")


class ClsTokuisaki():
    def GetbyID(session, tokuisakicd):
        logger.info("ClsTokuisaki.GetbyID 処理開始")

        # query定義
        query = "SELECT * FROM TM得意先 WHERE 得意先CD = '{}'".format(tokuisakicd)
        result = dict(SQLExecutor(session).execute_query(query=query))
        return result['results']

class ClsShiiresaki():
    def GetbyID(session, shiiresakicd):
        logger.info("ClsShiiresaki.GetbyID 処理開始")

        # query定義
        query = "SELECT * FROM TM仕入先 WHERE 仕入先CD = '{}'".format(shiiresakicd)
        result = dict(SQLExecutor(session).execute_query(query=query))
        return result['results']

class ClsTanto():
    def GetbyID(self,session,tantocd):
        logger.info("ClsTanto.GetbyID 処理開始")

        #query定義
        query = "SELECT * FROM TM担当者 WHERE 担当者CD = {}".format(tantocd)
        result = dict(SQLExecutor(session).execute_query(query=query))
        return result['results']


class ClsKaisya():
    def GetData(self,session):
        logger.info("ClsKaisya.GetData 処理開始")

        #query定義
        query = "SELECT * FROM TM会社 WHERE 会社ID = '01'"
        result = dict(SQLExecutor(session).execute_query(query=query))
        return result['results']

class ClsToukeiSyukei():
    def GetbyID(session, syukeisakicd):
        logger.info("ClsToukeiSyukei.GetbyID 処理開始")

        # query定義
        query = "SELECT * FROM TM統計集計先 WHERE 統計集計先CD = '{}'".format(syukeisakicd)
        result = dict(SQLExecutor(session).execute_query(query=query))
        return result['results']



class ClsMitumoriH():
    def __init__(self,estimate_no,mitumori_category = 2,syanaiden_category = 0):
        self.estimate_category = mitumori_category
        self.syanaiden = syanaiden_category
        self.mitumori_no = estimate_no

    def IsUCategory(self,session,category) -> bool:

        query = """
            SELECT COUNT(U区分) AS 区分カウント
            FROM TD見積シートM
            WHERE 見積番号 = {}
            AND U区分 = '{}'
            AND 見積数量 <> 0
        """.format(self.mitumori_no,category)

        result = dict(SQLExecutor(session).execute_query(query=query))

        if result["count"] == 0:
            return False
        if result["results"][0]["区分カウント"] == 0:
            return False
        else:
            return True

    def GetbyID(self, session):
        query = "SELECT * FROM TD見積 WHERE 見積番号 = {}".format(self.mitumori_no)
        result = dict(SQLExecutor(session).execute_query(query=query))

        mitsumorikubun_dic = {
            "karimitsumori":0,
            "honmitsumori":1,
            "subete":2
        }

        syanaiden_dic = {
            "subete":0,
            "syanaiden_igai":1,
            "syanaiden_nomi":2
        }

        if self.estimate_category == mitsumorikubun_dic['karimitsumori']:
            if result['results'][0]['見積区分'] == mitsumorikubun_dic['honmitsumori']:
                raise ServiceError('指定の見積番号は本見積ではありません。')
        if self.estimate_category == mitsumorikubun_dic['honmitsumori']:
            if result['results'][0]['見積区分'] == mitsumorikubun_dic['karimitsumori']:
                raise ServiceError('指定の見積番号は仮見積ではありません。')

        if self.syanaiden == syanaiden_dic['syanaiden_igai']:
            if result['results'][0]['得意先CD'] == 9999:
                raise ServiceError('社内在庫分です。')
        if self.syanaiden == syanaiden_dic['syanaiden_nomi']:
            if result['results'][0]['得意先CD'] != 9999:
                raise ServiceError('社内在庫以外です。')

        result = result['results'][0]

        self.見積番号 = self.mitumori_no
        self.担当者CD = result["担当者CD"]
        self.見積日付 = result["見積日付"]
        self.見積件名 = result["見積件名"]
        self.得意先CD = null_to_zero(result["得意先CD"], "")
        self.得意先名1 = null_to_zero(result["得意先名1"], "")
        self.得意先名2 = null_to_zero(result["得意先名2"], "")
        self.得TEL = null_to_zero(result["得TEL"], "")
        self.得FAX = null_to_zero(result["得FAX"], "")
        self.得担当者 = null_to_zero(result["得意先担当者"], "")
        self.納得意先CD = null_to_zero(result["納入得意先CD"], "")
        self.納入先CD = null_to_zero(result["納入先CD"], "")
        self.納入先名1 = null_to_zero(result["納入先名1"], "")
        self.納入先名2 = null_to_zero(result["納入先名2"], "")
        self.納郵便番号 = null_to_zero(result["郵便番号"], "")
        self.納住所1 = null_to_zero(result["住所1"], "")
        self.納住所2 = null_to_zero(result["住所2"], "")
        self.納TEL = null_to_zero(result["納TEL"], "")
        self.納FAX = null_to_zero(result["納FAX"], "")
        self.納担当者 = null_to_zero(result["納入先担当者"], "")
        self.納期S = result["納期S"]
        self.納期E = result["納期E"]
        self.備考 = null_to_zero(result["備考"], "")
        self.規模金額 = null_to_zero(result["物件規模金額"], "")
        self.OPEN日 = result["オープン日"]
        self.物件種別 = null_to_zero(result["物件種別"], "")
        self.現場名 = null_to_zero(result["現場名"], "")
        self.支払条件 = null_to_zero(result["支払条件"], "")
        self.支払条件他 = null_to_zero(result["支払条件その他"], "")
        self.納期表示 = null_to_zero(result["納期表示"], "")
        self.納期表示他 = null_to_zero(result["納期その他"], "")
        self.見積日出力 = null_to_zero(result["見積日出力"], "")
        self.有効期限 = null_to_zero(result["有効期限"], "")
        self.受注区分 = null_to_zero(result["受注区分"], "")
        self.受注日付 = result["受注日付"]
        self.大小口区分 = null_to_zero(result["大小口区分"], "")
        self.出精値引 = null_to_zero(result["出精値引"], "")

        self.受注日付 = result["税集計区分"]
        self.売上端数 = result["売上端数"]
        self.消費税端数 = result["消費税端数"]

        self.合計金額 = result["合計金額"]
        self.原価合計 = result["原価合計"]
        self.原価率 = result["原価率"]
        self.外税額 = result["外税額"]

        self.得意先別見積番号 = null_to_zero(result["得意先別見積番号"], "")

        self.見積区分 = null_to_zero(result["見積区分"], "")

        return True


class ClsDates():
    def GetbyId(self,session,DateId):
        query = "select * from TMDates where DateID = '{}'".format(DateId)
        result = dict(SQLExecutor(session).execute_query(query=query))

        if result["count"] == 0:
            self.update_date = '1990/01/01'
            return False
        else:
            self.DateID = result["results"][0]["DateID"]
            self.更新日付 = result["results"][0]["更新日付"]
            return True

class ClsZeiritsu():
    def GetbyID(self,session,biling_date):
        query = "select * from TM税率 where 税率ID = 'ZR'"
        result = dict(SQLExecutor(session).execute_query(query=query))
        res_flg = False
        if result["count"] == 0:
            res_flg = False

            self.税率 = 0
            self.切替日付 = None
            self.旧税率 = 0
        else:
            res_flg  = True
            result = result["results"][0]
            self.税率ID = result["税率ID"]
            self.税率 = result["税率"]
            self.切替日付 = result["切替日付"]
            self.旧税率 = result["旧税率"]

        return res_flg

    def GetbyDate(self,session,billing_date):
        query = "select * from TM税率 where 税率ID = 'ZR'"
        result = dict(SQLExecutor(session).execute_query(query=query))
        res = 0
        if result["count"] != 0:
            change_date = result["results"][0]["切替日付"]
            billing_date = billing_date
            if change_date <= billing_date:
                res = result["results"][0]["税率"]
            elif change_date > billing_date:
                res = result["results"][0]["旧税率"]

        return res

class ClsOutputRireki():
    def SetOutputLog(self,session,list_name,number,pc_name = None) -> bool:

        stored_name = "usp_見積出力履歴更新"

        params = {
            "@iListName":list_name,
            "@iNumber":number
        }
        if pc_name != None:
            params["@iPCName"] = pc_name

        outputparams = {"@RetST":"INT", "@RetMsg":"VARCHAR(255)"}

        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=stored_name, params=params,outputparams=outputparams))

        if storedresults["output_values"]["@RetST"] != 0:
            raise ServiceError("{}：{}".format(storedresults["output_values"]["@RetST"],storedresults["output_values"]["@RetMsg"]))

        return True
    
class Snw_cm():
    def GetCounter(self, session, sItemName:str, sTokuID:str = ""):
        storedname = "usp_GetCounter"

        params = {
            "@iItemName":sItemName,
            "@iTokuID":sTokuID
        }
        
        outputparams = {"@GetNO":"INT"}

        storedresults = dict(SQLExecutor(session).execute_stored_procedure(storedname=storedname, params=params,outputparams=outputparams))

        return storedresults

class ClsKamoku():
    def __init__(self,kamoku_cd):
        self.科目CD = kamoku_cd

    def GetbyID(self,session) -> bool:

        query = "SELECT * FROM TM科目 WHERE 科目CD = {}".format(self.科目CD)
        result = dict(SQLExecutor(session).execute_query(query=query))

        logger.debug(result)

        if result["count"] == 0:
            GetByID = False
            self.科目CD = ""
            self.科目名 = ""
            self.借方消費税区分 = ""
            self.貸方消費税区分 = ""
            self.順序 = 0
            self.集計CD = 0
            self.按分区分 = 0

        else:

            GetByID = True
            self.科目CD = result["results"][0]['科目CD']
            self.科目名 = result["results"][0]['科目名']
            self.借方消費税区分 = result["results"][0]['借方消費税区分']
            self.貸方消費税区分 = result["results"][0]['貸方消費税区分']
            self.順序 = result["results"][0]['順序']
            self.集計CD = result["results"][0]['集計CD']
            self.按分区分 = result["results"][0]['按分区分']

        return GetByID

class ClsConvTanto():
    def __init__(self,zaimutantosya_cd):
        self.財務応援担当者CD = zaimutantosya_cd

    def ConvertByID(self,session):
        if self.財務応援担当者CD == 999:
            self.m_積算担当者CD = ""
        elif self.財務応援担当者CD == "":
            self.m_積算担当者CD = ""
        else:
            if self.GetbyId(session,self.財務応援担当者CD) == False:
                self.m_積算担当者CD = ""

    def GetbyId(self,session,zaimutantosya_cd) -> bool:
        query = "SELECT * FROM TM担当者変換 WHERE 財務応援担当者CD = {}".format(zaimutantosya_cd)
        result = dict(SQLExecutor(session).execute_query(query=query))

        if result["count"] == 0:
            GetbyID = False
            self.m_積算担当者CD = ""
            self.m_財務応援担当者CD = ""

        else:
            GetbyID = True
            self.m_積算担当者CD = result["results"][0]['担当者CD']
            self.m_財務応援担当者CD = result["results"][0]['財務応援担当者CD']

        return GetbyID
