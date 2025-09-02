import tkinter as tk
from tkinter import ttk, filedialog, messagebox
import config
import util
import threading
import os
import json
import logging

UI_WIDTH = 1000
UI_HEIGHT = 750
UI_LONG_INPUT_WIDTH = 80
UI_SHORT_INPUT_WIDTH = 20
UI_LOG_WIDTH = 100


class Gui(tk.Tk):
    def __init__(self, title, execute_callback=None, save_callback=None):
        super().__init__()
        self.title(title)
        self.geometry(f"{UI_WIDTH}x{UI_HEIGHT}")
        self.grid_columnconfigure(1, weight=1)
        self.grid_rowconfigure(12, weight=1)



        self.radio_api = tk.StringVar(value=config.DATA_OPTIONS["POSTリクエスト"])
        self.radio_action = tk.StringVar(value=config.ENTRY_OPTIONS["サイト新規作成"])
        self.is_create = True
        self.is_copy = False
        self.execute_callback = execute_callback
        self.save_callback = save_callback
        self.show_password = tk.BooleanVar(value=False)

        # Initialize input fields
        self.entry_api_key = None
        self.entry_server_address = None
        self.entry_site_id = None
        self.entry_input_dir = None
        self.entry_package_file = None

        # Initialize checkbox variables
        self.cbs = {}

        self.create_widgets()
        self.initialize_default_values()

        # キーボードバインディングを追加する
        self.bind('<Alt-s>', lambda event: self.start_save())
        self.bind('<Alt-e>', lambda event: self.start_execute())

    def create_widgets(self):
        padding = {'padx': 10, 'pady': 5}

        row_index = 0
        # POSTリクエストとJSON出力用フレーム
        # api_frame = tk.Frame(self)
        # api_frame.grid(row=row_index, column=0, columnspan=3, **padding)

        # self.radioApi = tk.Radiobutton(api_frame, text="POSTリクエスト", variable=self.radio_api, value=config.DATA_OPTIONS["POSTリクエスト"], command=self.toggle_entries)
        # self.radioApi.pack(side=tk.LEFT, **padding)

        # self.radioJson = tk.Radiobutton(api_frame, text="JSON出力", variable=self.radio_api, value=config.DATA_OPTIONS["JSON出力"], command=self.toggle_entries)
        # self.radioJson.pack(side=tk.LEFT, **padding)

        # row_index += 1
        # サイト新規作成とサイト更新用フレーム
        action_frame = tk.Frame(self)
        action_frame.grid(row=row_index, column=0, columnspan=3, **padding)

        self.radioCreate = tk.Radiobutton(action_frame, text="サイト新規作成", variable=self.radio_action, value=config.ENTRY_OPTIONS["サイト新規作成"], command=self.toggle_entries)
        self.radioCreate.pack(side=tk.LEFT, **padding)

        self.radioUpdate = tk.Radiobutton(action_frame, text="サイトコピー", variable=self.radio_action, value=config.ENTRY_OPTIONS["サイトコピー"], command=self.toggle_entries)
        self.radioUpdate.pack(side=tk.LEFT, **padding)

        # Add a blank row
        row_index += 1
        tk.Frame(self).grid(row=row_index, column=0, columnspan=3, pady=5)

        # 入力フィールド
        row_index += 1
        self.entry_api_key = self.create_labeled_entry("APIキー", row_index, show="*")
        self.toggle_password_btn = tk.Checkbutton(self, text="表示", variable=self.show_password, command=self.toggle_password_visibility)
        self.toggle_password_btn.grid(row=row_index, column=2, **padding)

        row_index += 1
        label = tk.Label(self, text="サイト名", anchor='e')
        label.grid(row=row_index, column=0, sticky='e', padx=(10, 5), pady=5)

        site_frame = tk.Frame(self)
        site_frame.grid(row=row_index, column=1, sticky='w', padx=(0, 10), pady=5)

        self.entry_server_address = tk.Entry(site_frame, width=UI_LONG_INPUT_WIDTH - UI_SHORT_INPUT_WIDTH - 5)
        self.entry_server_address.pack(side=tk.LEFT)

        tk.Label(site_frame, text="/items/").pack(side=tk.LEFT)

        self.entry_site_id = tk.Entry(site_frame, width=UI_SHORT_INPUT_WIDTH)
        self.entry_site_id.pack(side=tk.LEFT)

        row_index += 1
        self.entry_input_dir = self.create_labeled_entry("入力設計書ディレクトリ", row_index)
        self.button_select_input = tk.Button(self, text="選択", command=self.select_input_folder)
        self.button_select_input.grid(row=row_index, column=2, **padding)

        # Add checkbox frame
        row_index += 1
        checkbox_frame = tk.Frame(self)
        checkbox_frame.grid(row=row_index, column=0, columnspan=3, **padding)

        for sheet_name, sheet_info in config.SHEETS.items():
            var = tk.BooleanVar()
            cb = tk.Checkbutton(checkbox_frame, text=sheet_name, variable=var)
            cb.pack(side=tk.LEFT, **padding)

            self.cbs[sheet_info["key"]] = (cb, var)

        # Add note about conforming to design documents
        note_label = tk.Label(checkbox_frame, text="※ 設計書に準拠する", font=("Helvetica", 8), fg="gray")
        note_label.pack(side=tk.LEFT, padx=(10, 0))

        row_index += 1
        self.entry_package_file = self.create_labeled_entry("入力サイトパッケージファイル", row_index)
        self.button_select_output = tk.Button(self, text="選択", command=self.select_output_file)
        self.button_select_output.grid(row=row_index, column=2, **padding)

        # 実行ボタン
        row_index += 1
        button_frame = tk.Frame(self)
        button_frame.grid(row=row_index, column=0, columnspan=3, **padding)
        self.execute_button = tk.Button(button_frame, text="実行(E)", command=self.start_execute)
        self.execute_button.pack(side=tk.LEFT, padx=5)

        # 保存ボタン
        self.save_button = tk.Button(button_frame, text="保存(S)", command=self.start_save)
        self.save_button.pack(side=tk.LEFT, padx=5)

        # プログレスバー
        row_index += 1
        self.spinner = ttk.Progressbar(self, mode='indeterminate')
        self.spinner.grid(row=row_index, column=0, columnspan=3, sticky='ew', **padding)

        # ログボックス
        row_index += 1
        self.log_box = tk.Text(self, width=UI_LOG_WIDTH, wrap=tk.WORD)
        self.log_box.grid(row=row_index, column=0, columnspan=3, sticky='nsew', **padding)

        scrollbar = tk.Scrollbar(self, orient=tk.VERTICAL, command=self.log_box.yview)
        scrollbar.grid(row=row_index, column=3, sticky='ns')
        self.log_box.config(yscrollcommand=scrollbar.set)

        self.configure_text_tags()

    def create_labeled_entry(self, label_text, row, **kwargs):
        label = tk.Label(self, text=label_text, anchor='e')
        label.grid(row=row, column=0, sticky='e', padx=(10, 5), pady=5)

        entry = tk.Entry(self, width=UI_LONG_INPUT_WIDTH, **kwargs)
        entry.grid(row=row, column=1, sticky='ew', padx=(0, 10), pady=5)

        return entry

    def get_checkbox_values(self):
        return {key: var.get() for key, (cb, var) in self.cbs.items()}

    def select_input_folder(self):
        folder_path = filedialog.askdirectory()
        if folder_path:
            self.entry_input_dir.delete(0, tk.END)
            self.entry_input_dir.insert(0, folder_path)

    def select_output_file(self):
        file_path = filedialog.askopenfilename()
        if file_path:
            self.entry_package_file.delete(0, tk.END)
            self.entry_package_file.insert(0, file_path)

    def toggle_password_visibility(self):
        if self.show_password.get():
            self.entry_api_key.config(show="")
        else:
            self.entry_api_key.config(show="*")

    def toggle_entries(self):
        # if self.radio_api.get() == config.DATA_OPTIONS["POSTリクエスト"]:
        #     self.entry_package_file.config(state=tk.DISABLED)
        #     self.button_select_output.config(state=tk.DISABLED)
        #     self.entry_api_key.config(state=tk.NORMAL)
        #     self.entry_server_address.config(state=tk.NORMAL)
        #     self.radioUpdate.config(state=tk.NORMAL)
        #     self.radioCreate.config(state=tk.NORMAL)
        # else:
        #     self.entry_package_file.config(state=tk.NORMAL)
        #     self.button_select_output.config(state=tk.NORMAL)
        #     self.entry_api_key.config(state=tk.DISABLED)
        #     self.entry_server_address.config(state=tk.DISABLED)
        #     self.radioUpdate.config(state=tk.DISABLED)
        #     self.radioCreate.config(state=tk.DISABLED)
        self.is_create = self.radio_action.get() == config.ENTRY_OPTIONS["サイト新規作成"]
        self.is_copy = self.radio_action.get() == config.ENTRY_OPTIONS["サイトコピー"]

        if self.is_create:
            for cb, var in self.cbs.values():
                var.set(True)
                cb.config(state=tk.DISABLED)

            self.entry_package_file.config(state=tk.DISABLED)
            self.button_select_output.config(state=tk.DISABLED)

        else:
            for cb, var in self.cbs.values():
                cb.config(state=tk.NORMAL)
            self.entry_package_file.config(state=tk.NORMAL)
            self.button_select_output.config(state=tk.NORMAL)

    def start_execute(self):
        self.spinner.start()
        threading.Thread(target=self.execute).start()

    def execute(self):
        self.load_all_input_values()
        if self.execute_callback:
            try:
                self.execute_callback(self)
            except Exception as e:
                self.log_output(f"Execution error: {e}", "error")
        self.spinner.stop()

    def start_save(self):
        self.spinner.start()
        threading.Thread(target=self.save).start()

    def save(self):
        self.load_all_input_values()
        if self.save_callback:
            try:
                self.save_callback(self)
            except Exception as e:
                self.log_output(f"Save error: {e}", "error")
        self.spinner.stop()

    def initialize_default_values(self):
        try:
            saved_setting = util.read_json('setting.json')

            self.entry_api_key.insert(0, saved_setting.get("API_KEY", ""))
            self.entry_server_address.insert(0, saved_setting.get("SERVER_ADDRESS", ""))
            self.entry_site_id.insert(0, saved_setting.get("SITE_ID", ""))
            self.entry_input_dir.insert(0, saved_setting.get("INPUT_DIRECTORY", ""))
            self.entry_package_file.insert(0, saved_setting.get("PACKAGE_FILE", ""))
            # self.radio_api.set(saved_setting.get("DATA_OPTION", ""))
            self.radio_action.set(saved_setting.get("ENTRY_OPTION", ""))
        except Exception as e:
            self.log_output(f"設定ファイルの読み込みに失敗しました: {str(e)}", "error")
        self.toggle_entries()

    def load_all_input_values(self):
        self.api_key_value = self.entry_api_key.get()
        self.server_value = self.entry_server_address.get()
        self.site_value = self.entry_site_id.get()
        self.input_dir_value = self.entry_input_dir.get()
        self.package_file_value = self.entry_package_file.get()
        # self.radio_api_value = self.radio_api.get()
        self.is_create = self.radio_action.get() == config.ENTRY_OPTIONS["サイト新規作成"]
        self.is_copy = self.radio_action.get() == config.ENTRY_OPTIONS["サイトコピー"]
        self.radio_action_value = self.radio_action.get()
        # Load checkbox values
        self.checkbox_values = self.get_checkbox_values()

    def configure_text_tags(self):
        self.log_box.tag_configure("error", foreground="red")
        self.log_box.tag_configure("warning", foreground="orange")

    def log_output(self, message, msg_type="info"):
        # GUI上のログボックスにも表示
        if msg_type == "error":
            self.log_box.insert(tk.END, message + "\n", "error")
            logging.error(message)
            util.print_color(message, "red")
        elif msg_type == "warning":
            self.log_box.insert(tk.END, message + "\n", "warning")
            logging.warning(message)
            util.print_color(message, "yellow")
        elif msg_type == "debug":
            logging.debug(message)
        else:
            self.log_box.insert(tk.END, message + "\n")
            logging.info(message)
            print(message)

        # 最下部にスクロール
        self.log_box.see(tk.END)

    def show_message(self, title, message, message_type="info"):
        if message_type == "error":
            messagebox.showerror(title, message)
        elif message_type == "warning":
            messagebox.showwarning(title, message)
        else:
            messagebox.showinfo(title, message)

if __name__ == "__main__":
    def execute_callback(gui_instance):
        # Accessing values from the GUI instance
        print("API Key:", gui_instance.api_key_value)
        print("Server:", gui_instance.server_value)
        print("Site:", gui_instance.site_value)
        print("Input Directory:", gui_instance.input_dir_value)
        print("Output Directory:", gui_instance.package_file_value)
        # print("Radio API Value:", gui_instance.radio_api_value)


if __name__ == "__main__":
    import main
    app = Gui("設計書からサイトパッケージ作成",execute_callback=main.main, save_callback=main.save_setting)
    app.mainloop()