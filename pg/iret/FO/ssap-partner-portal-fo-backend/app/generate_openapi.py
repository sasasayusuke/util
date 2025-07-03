import argparse
import yaml

from uvicorn.importer import import_from_string

parser = argparse.ArgumentParser(prog="extract-openapi.py")
parser.add_argument(
    "--output", help="Output file ending in .yaml", default="openapi.yaml"
)

if __name__ == "__main__":
    args = parser.parse_args()

    app = import_from_string("main:app")
    openapi = app.openapi()
    version = openapi.get("openapi", "unknown version")

    print(f"writing openapi spec v{version}")
    with open(args.output, "w") as f:
        yaml.dump(openapi, f, sort_keys=False)
