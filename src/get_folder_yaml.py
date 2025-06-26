import os
import yaml

TARGET_DIR = r"/root/services/1on1"

EXCLUDED_FOLDERS = [
    '.history', '.zip', '.git', '.svn', '.next', '__pycache__',
    'node_modules', 'gitlab', 'venv', 'history', 'dist',
]

EXCLUDED_EXTENSIONS = [
    ".xls", ".xlsx", ".xlsm", ".ico", ".svg", ".png", ".jpg",
    ".mp3", ".mp4", ".LICENSE.txt", ".exe", ".vbw", ".frx"
]

EXCLUDED_FILES = [
    "package-lock.json", "bundle.js"
]

def should_exclude(path, file=None):
    if any(part in EXCLUDED_FOLDERS for part in path.split(os.sep)):
        return True
    if file:
        if file in EXCLUDED_FILES:
            return True
        if any(file.endswith(ext) for ext in EXCLUDED_EXTENSIONS):
            return True
    return False

def get_file_structure_with_contents(base_path):
    file_tree = {}

    for root, dirs, files in os.walk(base_path):
        if should_exclude(root):
            continue

        rel_root = os.path.relpath(root, base_path)
        current = file_tree

        if rel_root != ".":
            for part in rel_root.split(os.sep):
                current = current.setdefault(part, {})

        for file in files:
            if should_exclude(root, file):
                continue

            file_path = os.path.join(root, file)
            try:
                with open(file_path, "r", encoding="utf-8") as f:
                    content = f.read()
                current[file] = content
            except Exception as e:
                current[file] = f"<<読み取り失敗: {e}>>"

    return file_tree

if __name__ == "__main__":
    structure = get_file_structure_with_contents(TARGET_DIR)
    with open("output.yml", "w", encoding="utf-8") as f:
        yaml.dump(structure, f, allow_unicode=True, sort_keys=False, default_flow_style=False)
    print("✅ output.yml に保存しました。")
