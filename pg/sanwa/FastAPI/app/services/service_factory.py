import logging
from datetime import datetime
# from FastAPI.app.services.seikyu_urikake import seikyusyo_kose
# from FastAPI.app.services.seikyu_urikake import seikyusyoKose
from app.core.config import settings

# サービスクラスのインポート
from app.services.test.test import TestService
from app.services.mitsumori_hacchu.syukkalist import SyukkalistService
from app.services.mitsumori_hacchu.sekouiraisho import SekouiraisyoService
from app.services.mitsumori_hacchu.hattyusyo import hattyusyoService
from app.services.mitsumori_hacchu.tyumonsyo import tyumonsyoService
from app.services.mitsumori_hacchu.mitsumoriiraisyo import MitsumoriIraisyoService
from app.services.mitsumori_hacchu.mitsumorisyo import MitsumorisyoService
from app.services.mitsumori_hacchu.mitsumorisyoOkugaikoukokubutsusenyou import MitsumorisyoOkugaikoukokubutsusenyouService
from app.services.mitsumori_hacchu.mitsumorisyutsuryokukose import MitsumoriSyutsuryokuKoseService
from app.services.mitsumori_hacchu.keikakakuninhyou import KeikakakuninhyouService
from app.services.mitsumori_hacchu.partsList import PartsListService
from app.services.mitsumori_hacchu.storingList import StoringListService
from app.services.mitsumori_hacchu.hacchuithiran import HacchuithiranService
from app.services.uriage_nyukin.nyukinyoteihyo import NyukinyoteihyoService
from app.services.uriage_nyukin.dailySalesLedger import DailySalesLedgerService
from app.services.uriage_nyukin.dailyDeposit import DailyDepositService
from app.services.uriage_nyukin.retractedList import RetractedListService
from app.services.uriage_nyukin.uncollectedList import UncollectedListService
from app.services.uriage_nyukin.nouhinsyo import NouhinsyoService
from app.services.uriage_nyukin.nyukinkesikominyuryoku import nyukinkesikominyuryokuService
from app.services.uriage_nyukin.uriageNyuryoku import uriageNyuryokuService
from app.services.seikyu_urikake.seikyumeisaiitiranhyo import SeikyumeisaiitiranhyoService
from app.services.seikyu_urikake.joytechseikyumeisaiitiranhyo import joytechSeikyumeisaiitiranhyoService
from app.services.seikyu_urikake.mmseikyumeisaihyo import mmseikyumeisaihyoService
from app.services.seikyu_urikake.yaokoseikyumeisaihyo import yaokoSeikyumeisaihyoService
from app.services.seikyu_urikake.welciaseikyumeisai import welciaSeikyumeisaiService
from app.services.seikyu_urikake.urikakemeisaiitiranhyo import urikakemeisaiitiranhyoService
from app.services.seikyu_urikake.welciatyunitisekousyukeihyo import welciatyunitisekousyukeihyoService
from app.services.seikyu_urikake.seikyugenkarituhyo import seikyugenkarituhyoService
from app.services.seikyu_urikake.simamuraTyokunoSeikyusyo import simamuraTyokunoDenpyoSeikyusyoService
from app.services.seikyu_urikake.seikyusyoOkugai import OkugaiSeikyusyoService
from app.services.seikyu_urikake.seikyusyoKose import SeikyusyoForKoseService
from app.services.seikyu_urikake.seikyuitiranhyo import seikyuitiranhyoService
from app.services.seikyu_urikake.urikakekinmototyo import UrikakekinmototyoService
from app.services.seikyu_urikake.urikakekinsyukeihyo import UrikakekinsyukeihyoService
from app.services.seikyu_urikake.seikyusyo import SeikyusyoService
from app.services.seikyu_urikake.uriagemaeSeikyusyo import uriagemaeSeikyusyoService
from app.services.seikyu_urikake.monthlyClosing import MonthlyClosingService
from app.services.seikyu_urikake.tenyuuryokukagami import TenyuuryokukagamiService
from app.services.seikyu_urikake.urikakesyohizeityoseinyuryoku import urikakesyohizeityoseinyuryokuService
from app.services.siire_shiharai.shiharaiyoteihyo import ShiharaiyoteihyoService
from app.services.siire_shiharai.shiireNyuryoku import ShiireNyuryokuService
from app.services.siire_shiharai.shiharaidatatorikomi import shiharaidatatorikomiService
from app.services.siire_shiharai.siharaiNyuryoku import siharaiNyuryokuService
from app.services.siire_shiharai.shiirenikkeihyo import ShiirenikkeihyoService
from app.services.siire_shiharai.shiharainikkeihyo import ShiharainikkeihyoService
from app.services.shiharai_kaikake.shiiresakikensyumeisaihyo import shiiresakikensyumeisaihyoService
from app.services.shiharai_kaikake.monthlyDiscrepancyCheckhyo import monthlyDiscrepancyCheckhyoService
from app.services.shiharai_kaikake.kensyuitiranhyo import kensyuitiranhyoService
from app.services.shiharai_kaikake.kaikakekinmototyo import KaikakekinmototyoService
from app.services.shiharai_kaikake.kaikakekinsyukeihyo import KaikakekinsyukeihyoService
from app.services.toukei.claim_syukei import claimSyukeihyoService
from app.services.toukei.tokuisakibetunenkanuriagezyunisuiihyo import tokuisakibetunenkanuriagezyunisuiihyoService
from app.services.toukei.siiresakibetunenkansiirezyunisuiihyo import siiresakibetunenkansiirezyunisuiihyoService
from app.services.toukei.toukeisuiihyo import toukeisuiihyoService
from app.services.toukei.toukeisuiihyo_date import toukeisuiihyoDateService
from app.services.toukei.teamtsumiagecheckhyo import teamtsumiagecheckhyoService
from app.services.toukei.constructionValueByClient import ConstructionValueByClientService
from app.services.keihi.keihinyuryoku import keihinyuryokuService
from app.services.keihi.anbunkeihisyukeihyo import anbunKeihiSyukeiHyoService
from app.services.keihi.keihitorikomi import keihitorikomiService
from app.services.keihi.excelShiwakeTxtShuturyoku import ExcelShiwakeTxtShuturyokuService
from app.services.hosyu.seihinNohenkou import SeihinNohenkouService
from app.services.hosyu.deleteLockData import DeleteLockDataService
from app.services.master.sanwaServiceInput import SanwaServiceInputService




# 名前を指定してロガーを取得する
logger = logging.getLogger(settings.APP_NAME)

# サービス情報をクラス参照で設定
SERVICE_INFO = {
    "test": {
        "test": {
            "class": TestService,
            "buttons": {
                "xlsx": {
                    "handler": TestService.display,
                    "description": "TestExcelを出力します",
                    "auth": True,
                },
                "pdf": {
                    "handler": TestService.displayPDF,
                    "description": "TestPDFを出力します",
                    "auth": True,
                },

            },
        },
    },
    "見積・発注": {
        "出荷リスト出力（梱包出荷用）": {
            "class": SyukkalistService,
            "buttons": {
                "出力": {
                    "handler": SyukkalistService.display,
                    "description": "出荷リスト（梱包出荷用）を出力します",
                    "auth": True,
                },
            },
        },
        "施工依頼書": {
            "class": SekouiraisyoService,
            "buttons": {
                "出力": {
                    "handler": SekouiraisyoService.display,
                    "description": "施工依頼書を出力します",
                    "auth": True,
                },
            },
        },
        "発注書出力": {
            "class": hattyusyoService,
            "buttons": {
                "出力": {
                    "handler": hattyusyoService.display,
                    "description": "発注書を出力します",
                    "auth": True,
                },
            },
        },
        "注文書出力": {
            "class": tyumonsyoService,
            "buttons": {
                "出力": {
                    "handler": tyumonsyoService.display,
                    "description": "注文書を出力します",
                    "auth": True,
                },
            },
        },
        "見積依頼書出力": {
            "class": MitsumoriIraisyoService,
            "buttons": {
                "出力": {
                    "handler": MitsumoriIraisyoService.display,
                    "description": "見積依頼書を出力します",
                    "auth": True,
                },
            },
        },
        "見積書出力": {
            "class": MitsumorisyoService,
            "buttons": {
                "出力": {
                    "handler": MitsumorisyoService.display,
                    "description": "見積書を出力します",
                    "auth": True,
                },
            },
        },
        "見積書出力（屋外広告物専用）": {
            "class": MitsumorisyoOkugaikoukokubutsusenyouService,
            "buttons": {
                "出力": {
                    "handler": MitsumorisyoOkugaikoukokubutsusenyouService.display,
                    "description": "見積書（屋外広告物専用）を出力します",
                    "auth": True,
                },
            },
        },
        "見積書出力（コーセー専用）": {
            "class": MitsumoriSyutsuryokuKoseService,
            "buttons": {
                "出力": {
                    "handler": MitsumoriSyutsuryokuKoseService.display,
                    "description": "見積書出力（コーセー専用）を出力します",
                    "auth": True,
                },
            },
        },
        "経過確認表出力": {
            "class": KeikakakuninhyouService,
            "buttons": {
                "出力": {
                    "handler": KeikakakuninhyouService.display,
                    "description": "経過確認表を出力します",
                    "auth": True,
                },
            },
        },
        "部材リスト（一覧）出力": {
            "class": PartsListService,
            "buttons": {
                "出力": {
                    "handler": PartsListService.display,
                    "description": "部材リストを出力します",
                    "auth": True,
                },
            },
        },
        "入庫リスト作成": {
            "class": StoringListService,
            "buttons": {
                "出力": {
                    "handler": StoringListService.display,
                    "description": "入庫リストを出力します",
                    "auth": True,
                },
            },
        },
        "発注一覧表出力": {
            "class": HacchuithiranService,
            "buttons": {
                "出力": {
                    "handler": HacchuithiranService.display,
                    "description": "発注一覧表出力を出力します",
                    "auth": True,
                },
            },
        },
    },
    "仕入・支払":{
        "仕入入力":{
            "class":ShiireNyuryokuService,
            "buttons":{
                "明細内訳": {
                    "handler": ShiireNyuryokuService.render,
                    "description": "仕入明細入力を別タブで開きます",
                    "auth": True,
                },
                "チェック": {
                    "handler": ShiireNyuryokuService.check,
                    "description": "仕入明細登録前チェックを行います",
                    "auth": True,
                },
                "登録": {
                    "handler": ShiireNyuryokuService.upload,
                    "description": "仕入明細登録を行います",
                    "auth": True,
                },
                "全削除": {
                    "handler": ShiireNyuryokuService.delete,
                    "description": "表示されている明細情報をすべて削除します",
                    "auth": True,
                },

            }
        },
        "支払予定表":{
            "class":ShiharaiyoteihyoService,
            "buttons":{
                "印刷": {
                    "handler": ShiharaiyoteihyoService.display,
                    "description": "支払予定を一覧表示します",
                    "auth": True,
                }
            }
        },
        "支払データ取込処理":{
            "class":shiharaidatatorikomiService,
            "buttons":{
                "実行": {
                    "handler": shiharaidatatorikomiService.execute,
                    "description": "支払データ取込処理を実行します",
                    "auth": True,
                }
            }
        },
        "仕入日計表":{
            "class":ShiirenikkeihyoService,
            "buttons":{
                "印刷": {
                    "handler": ShiirenikkeihyoService.display,
                    "description": "仕入日計表を出力します",
                    "auth": True,
                }
            }
        },
        "支払入力":{
            "class":siharaiNyuryokuService,
            "buttons":{
                "保存": {
                    "handler": siharaiNyuryokuService.execute,
                    "description": "支払入力登録処理を実行します",
                    "auth": True,
                },
                "削除": {
                    "handler": siharaiNyuryokuService.purge,
                    "description": "支払入力削除処理を実行します",
                    "auth": True,
                }
            }
        },
        "支払日計表":{
            "class":ShiharainikkeihyoService,
            "buttons":{
                "印刷": {
                    "handler": ShiharainikkeihyoService.display,
                    "description": "支払日計表を出力します",
                    "auth": True,
                },
                "仕訳出力": {
                    "handler": ShiharainikkeihyoService.display_siwake,
                    "description": "支払仕訳データを出力します",
                    "auth": True,
                },
            }
        },
    },
    "売上・入金": {
        "入金予定表": {
            "class": NyukinyoteihyoService,
            "buttons": {
                "印刷": {
                    "handler": NyukinyoteihyoService.display,
                    "description": "入金予定を一覧表示します",
                    "auth": True,
                },
                "実行": {
                    "handler": NyukinyoteihyoService.execute,
                    "description": "入金を実行します",
                    "auth": True,
                }
            },
        },
        "売上日計表": {
            "class": DailySalesLedgerService,
            "buttons": {
                "印刷": {
                    "handler": DailySalesLedgerService.display,
                    "description": "売上日計表を出力します",
                    "auth": True,
                },
            },
        },
        "入金日計表": {
            "class": DailyDepositService,
            "buttons": {
                "印刷": {
                    "handler": DailyDepositService.display,
                    "description": "入金日計表を出力します",
                    "auth": True,
                },
            },
        },
        "未回収一覧表": {
            "class": UncollectedListService,
            "buttons": {
                "印刷": {
                    "handler": UncollectedListService.display,
                    "description": "未回収一覧表を出力します",
                    "auth": True,
                },
            },
        },
        "消込済一覧表": {
            "class": RetractedListService,
            "buttons": {
                "印刷": {
                    "handler": RetractedListService.display,
                    "description": "消込済一覧表を出力します",
                    "auth": True,
                },
            },
        },
        "納品書発行": {
            "class": NouhinsyoService,
            "buttons": {
                "出力": {
                    "handler": NouhinsyoService.display,
                    "description": "納品書を出力します",
                    "auth": True,
                },
                "物品出力": {
                    "handler": NouhinsyoService.display_buppin,
                    "description": "物品受領書を出力します",
                    "auth": True,
                }
            },
        },
        "入金消込入力": {
            "class": nyukinkesikominyuryokuService,
            "buttons": {
                "保存": {
                    "handler": nyukinkesikominyuryokuService.execute,
                    "description": "入金消込入力を保存します",
                    "auth": True,
                },
                "削除": {
                    "handler": nyukinkesikominyuryokuService.purge,
                    "description": "入金消込入力を削除します",
                    "auth": True,
                },
            },
        },
        "売上入力": {
            "class": uriageNyuryokuService,
            "buttons": {
                "明細内訳": {
                    "handler": uriageNyuryokuService.render,
                    "description": "売上明細を描画します",
                    "auth": True,
                },
                "登録": {
                    "handler": uriageNyuryokuService.upload,
                    "description": "売上明細を登録します",
                    "auth": True,
                },
                "削除チェック": {
                    "handler": uriageNyuryokuService.purge_check,
                    "description": "売上明細を削除チェックします",
                    "auth": True,
                },
                "削除": {
                    "handler": uriageNyuryokuService.purge,
                    "description": "売上明細を削除チェックします",
                    "auth": True,
                },
            },
        }
    },
    "請求・売掛": {
        "請求明細一覧表": {
            "class": SeikyumeisaiitiranhyoService,
            "buttons": {
                "出力": {
                    "handler": SeikyumeisaiitiranhyoService.display,
                    "description": "請求明細一覧表を出力します",
                    "auth": True,
                }
            },
        },
        "ジョイテック請求明細一覧表": {
            "class": joytechSeikyumeisaiitiranhyoService,
            "buttons": {
                "出力": {
                    "handler": joytechSeikyumeisaiitiranhyoService.display,
                    "description": "ジョイテック請求明細一覧表を出力します",
                    "auth": True,
                }
            },
        },
        "マミーマート請求明細表": {
            "class": mmseikyumeisaihyoService,
            "buttons": {
                "出力": {
                    "handler": mmseikyumeisaihyoService.display,
                    "description": "マミーマート請求明細表を出力します",
                    "auth": True,
                }
            },
        },
        "ヤオコー請求明細表": {
            "class": yaokoSeikyumeisaihyoService,
            "buttons": {
                "出力": {
                    "handler": yaokoSeikyumeisaihyoService.display,
                    "description": "ヤオコー請求明細一覧表を出力します",
                    "auth": True,
                }
            },
        },
        "ウエルシア請求明細": {
            "class": welciaSeikyumeisaiService,
            "buttons": {
                "出力": {
                    "handler": welciaSeikyumeisaiService.display,
                    "description": "ウエルシア請求明細表を出力します",
                    "auth": True,
                }
            },
        },
        "売掛明細一覧表": {
            "class": urikakemeisaiitiranhyoService,
            "buttons": {
                "出力": {
                    "handler": urikakemeisaiitiranhyoService.display,
                    "description": "売掛明細一覧表を出力します",
                    "auth": True,
                }
            },
        },
        "ウエルシア中日施工集計表": {
            "class": welciatyunitisekousyukeihyoService,
            "buttons": {
                "出力": {
                    "handler": welciatyunitisekousyukeihyoService.display,
                    "description": "ウエルシア中日施工集計表を出力します",
                    "auth": True,
                }
            },
        },
        "請求原価率表": {
            "class": seikyugenkarituhyoService,
            "buttons": {
                "出力": {
                    "handler": seikyugenkarituhyoService.display,
                    "description": "請求原価率表を出力します",
                    "auth": True,
                }
            },
        },
        "請求書発行　屋外広告物専用": {
            "class": OkugaiSeikyusyoService,
            "buttons": {
                "出力": {
                    "handler": OkugaiSeikyusyoService.display,
                    "description": "請求書発行　屋外広告物専用を出力します",
                    "auth": True,
                }
            },
        },
        "しまむら直納伝票請求書": {
            "class": simamuraTyokunoDenpyoSeikyusyoService,
            "buttons": {
                "出力": {
                    "handler": simamuraTyokunoDenpyoSeikyusyoService.display,
                    "description": "しまむら直納伝票請求書を出力します",
                    "auth": True,
                }
            },
        },
        "請求書発行　コーセー専用": {
            "class": SeikyusyoForKoseService,
            "buttons": {
                "出力": {
                    "handler": SeikyusyoForKoseService.display,
                    "description": "請求書発行　コーセー専用を出力します",
                    "auth": True,
                }
            },
        },
        "請求一覧表": {
            "class": seikyuitiranhyoService,
            "buttons": {
                "印刷": {
                    "handler": seikyuitiranhyoService.display,
                    "description": "請求一覧表を出力します",
                    "auth": True,
                },
            },
        },
        "売掛金元帳": {
            "class": UrikakekinmototyoService,
            "buttons": {
                "印刷": {
                    "handler": UrikakekinmototyoService.display,
                    "description": "売掛金元帳を出力します",
                    "auth": True,
                },
            },
        },
        "売掛金集計表": {
            "class": UrikakekinsyukeihyoService,
            "buttons": {
                "印刷": {
                    "handler": UrikakekinsyukeihyoService.display,
                    "description": "売掛金集計表を出力します",
                    "auth": True,
                },
                "仕訳出力": {
                    "handler": UrikakekinsyukeihyoService.display_siwake,
                    "description": "売掛金仕訳データを出力します",
                    "auth": True,
                },
            },
        },
        "請求書発行": {
            "class": SeikyusyoService,
            "buttons": {
                "出力": {
                    "handler": SeikyusyoService.display,
                    "description": "請求書を出力します",
                    "auth": True,
                },
                "削除": {
                    "handler": SeikyusyoService.delete,
                    "description": "請求書を削除します",
                    "auth": True,
                }
            },
        },
        "売掛消費税調整入力": {
            "class": urikakesyohizeityoseinyuryokuService,
            "buttons": {
                "保存": {
                    "handler": urikakesyohizeityoseinyuryokuService.execute,
                    "description": "売掛消費税調整入力を保存します",
                    "auth": True,
                },
                "削除": {
                    "handler": urikakesyohizeityoseinyuryokuService.delete,
                    "description": "売掛消費税調整入力を削除します",
                    "auth": True,
                }
            },
        },
        "売上前請求書発行": {
            "class": uriagemaeSeikyusyoService,
            "buttons": {
                "出力": {
                    "handler": uriagemaeSeikyusyoService.display,
                    "description": "売上前請求書を出力します",
                    "auth": True,
                },
                "削除": {
                    "handler": uriagemaeSeikyusyoService.delete,
                    "description": "売上前請求書を削除します",
                    "auth": True,
                }
            },
        },
        "月次締め処理": {
            "class": MonthlyClosingService,
            "buttons": {
                "実行": {
                    "handler": MonthlyClosingService.upload,
                    "description": "月次締め処理を実行します",
                    "auth": True,
                },
                "戻し": {
                    "handler": MonthlyClosingService.uploadBack,
                    "description": "月次締め戻し処理を実行します",
                    "auth": True,
                }
            },
        },
        "手入力鏡発行": {
            "class": TenyuuryokukagamiService,
            "buttons": {
                "保存": {
                    "handler": TenyuuryokukagamiService.display,
                    "description": "手入力鏡を登録し、出力します",
                    "auth": True,
                },
                "更新": {
                    "handler": TenyuuryokukagamiService.display_update,
                    "description": "手入力鏡を更新し、出力します",
                    "auth": True,
                },
                "削除": {
                    "handler": TenyuuryokukagamiService.display_delete,
                    "description": "手入力鏡の該当データを削除します",
                    "auth": True,
                },
            },
        },
    },
    "支払・買掛":{
        "仕入先検収明細表":{
            "class":shiiresakikensyumeisaihyoService,
            "buttons":{
                "出力": {
                    "handler": shiiresakikensyumeisaihyoService.display,
                    "description": "仕入先検収明細表を出力します",
                    "auth": True,
                }
            }
        },
        "月ズレチェック表":{
            "class":monthlyDiscrepancyCheckhyoService,
            "buttons":{
                "出力": {
                    "handler": monthlyDiscrepancyCheckhyoService.display,
                    "description": "月ズレチェック表を出力します",
                    "auth": True,
                }
            }
        },
        "検収一覧表":{
            "class":kensyuitiranhyoService,
            "buttons":{
                "印刷": {
                    "handler": kensyuitiranhyoService.display,
                    "description": "検収一覧表を出力します",
                    "auth": True,
                }
            }
        },
        "買掛金元帳":{
            "class":KaikakekinmototyoService,
            "buttons":{
                "印刷": {
                    "handler": KaikakekinmototyoService.display,
                    "description": "買掛金元帳を出力します",
                    "auth": True,
                }
            }
        },
        "買掛金集計表": {
            "class": KaikakekinsyukeihyoService,
            "buttons": {
                "印刷": {
                    "handler": KaikakekinsyukeihyoService.display,
                    "description": "買掛金集計表を出力します",
                    "auth": True,
                },
                "仕訳出力": {
                    "handler": KaikakekinsyukeihyoService.display_siwake,
                    "description": "買掛金仕訳データを出力します",
                    "auth": True,
                },
            },
        },
    },
    "統計":{
        "得意先別年間売上順位推移表":{
            "class":tokuisakibetunenkanuriagezyunisuiihyoService,
            "buttons":{
                "出力": {
                    "handler": tokuisakibetunenkanuriagezyunisuiihyoService.display,
                    "description": "得意先別年間売上順位推移表を出力します",
                    "auth": True,
                },
            }
        },
        "仕入先別年間仕入順位推移表":{
            "class":siiresakibetunenkansiirezyunisuiihyoService,
            "buttons":{
                "出力": {
                    "handler": siiresakibetunenkansiirezyunisuiihyoService.display,
                    "description": "仕入先別年間仕入順位推移表を出力します",
                    "auth": True,
                },
            }
        },
        "チーム別積上チェック表":{
            "class":teamtsumiagecheckhyoService,
            "buttons":{
                "出力": {
                    "handler": teamtsumiagecheckhyoService.display,
                    "description": "チーム別積上チェック表を出力します",
                    "auth": True,
                },
            }
        },
        "クレーム集計表":{
            "class":claimSyukeihyoService,
            "buttons":{
                "出力": {
                    "handler": claimSyukeihyoService.display,
                    "description": "クレーム集計表を出力します",
                    "auth": True,
                },
            }
        },
        "統計推移表":{
            "class":toukeisuiihyoService,
            "buttons":{
                "出力": {
                    "handler": toukeisuiihyoService.display,
                    "description": "統計推移表を出力します",
                    "auth": True,
                },
            }
        },
        "統計推移表（日単位）":{
            "class":toukeisuiihyoDateService,
            "buttons":{
                "出力": {
                    "handler": toukeisuiihyoDateService.display,
                    "description": "統計推移表_（日単位）を出力します",
                    "auth": True,
                },
            }
        },
        "顧客別施工金額実績表":{
            "class":ConstructionValueByClientService,
            "buttons":{
                "出力": {
                    "handler": ConstructionValueByClientService.display,
                    "description": "顧客別施工金額実績表を出力します",
                    "auth": True,
                },
            }
        },
    },
    "経費":{
        "経費入力":{
            "class":keihinyuryokuService,
            "buttons":{
                "保存": {
                    "handler": keihinyuryokuService.execute,
                    "description": "経費入力を保存します",
                    "auth": True,
                },
                "削除": {
                    "handler": keihinyuryokuService.delete,
                    "description": "経費入力を削除します",
                    "auth": True,
                }
            }
        },
        "按分経費集計表":{
            "class":anbunKeihiSyukeiHyoService,
            "buttons":{
                "出力": {
                    "handler": anbunKeihiSyukeiHyoService.display,
                    "description": "按分経費集計表を出力します",
                    "auth": True,
                },
            }
        },
        "経費取込処理":{
            "class":keihitorikomiService,
            "buttons":{
                "実行": {
                    "handler": keihitorikomiService.execute,
                    "description": "経費取込処理を実行します",
                    "auth": True,
                },
                "削除": {
                    "handler": keihitorikomiService.delete,
                    "description": "経費削除処理を実行します",
                    "auth": True,
                }
            }
        },
        "EXCEL仕訳TXT出力処理":{
            "class":ExcelShiwakeTxtShuturyokuService,
            "buttons":{
                "実行": {
                    "handler": ExcelShiwakeTxtShuturyokuService.execute,
                    "description": "EXCEL仕訳TXT出力処理を実行します",
                    "auth": True,
                },
            }
        },
    },
    "保守":{
        "製品No変更":{
            "class":SeihinNohenkouService,
            "buttons":{
                "実行": {
                    "handler": SeihinNohenkouService.update,
                    "description": "製品No変更を実行します",
                    "auth": True,
                },
            }
        },
        "ロックデータ削除":{
            "class":DeleteLockDataService,
            "buttons":{
                "取得": {
                    "handler": DeleteLockDataService.get,
                    "description": "ロックデータ取得処理を実行します",
                    "auth": True,
                },
                "削除": {
                    "handler": DeleteLockDataService.delete,
                    "description": "ロックデータ削除処理を実行します",
                    "auth": True,
                },
            }
        },
    },
    "マスタ":{
        "三和サービス実績入力":{
            "class":SanwaServiceInputService,
            "buttons":{
                "保存": {
                    "handler": SanwaServiceInputService.insert,
                    "description": "三和サービス実績入力を登録します",
                    "auth": True,
                },
                "更新": {
                    "handler": SanwaServiceInputService.update,
                    "description": "三和サービス実績入力を更新します",
                    "auth": True,
                },
                "削除": {
                    "handler": SanwaServiceInputService.delete,
                    "description": "三和サービス実績入力の該当データを削除します",
                    "auth": True,
                },
            }
        },
    }
}

class ServiceFactory:
    """サービスファクトリー"""

    def _get_service_instance(self, category: str, title: str):
        """カテゴリとタイトルに基づいてサービスインスタンスを取得"""
        service_class = SERVICE_INFO[category][title]["class"]
        return service_class()

    def handle_request(self, request, session, method='POST'):
        """
        指定されたボタンに応じたメソッドを実行し、レスポンスを受け取る

        Args:
            request: リクエストオブジェクト
            session: セッションオブジェクト
            method (str): HTTPメソッド 'GET' または 'POST'

        Returns:
            レスポンス
        """
        if method.upper() == 'GET':
            # GETの場合はクエリパラメータから取得
            category = request.query_params.get('category')
            title = request.query_params.get('title')
            button = request.query_params.get('button')
            opentime = request.query_params.get('opentime')
            button = request.query_params.get('button')
            params = request.query_params
        else:
            # POSTの場合
            category = request.category
            title = request.title
            button = request.button
            opentime = request.opentime
            params = request.params
        # サービスインスタンスを取得
        service_instance = self._get_service_instance(category, title)

        # SERVICE_INFOからボタン情報を取得し、対応するメソッドを呼び出す
        button_info = SERVICE_INFO[category][title]["buttons"].get(button)

        # 説明を取得して実行ログを出力
        description = button_info.get("description")
        logger.info(
            f"タイトル: {title} \n"
            f"| カテゴリ: {category} \n"
            f"| ボタン: {button} \n"
            f"| 説明: {description} \n"
            f"| ダイアログ起動時刻: {opentime} \n"
            f"| パラメータ: {params} \n"
        )
        # handlerメソッドを取得して実行
        handler = button_info.get("handler")
        return handler(service_instance, request=request, session=session)
