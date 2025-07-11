import sys, os

fail = False
for root, dirs, files in os.walk('.', topdown=True):
    for f in files:
        if f.endswith('.cs'):
            path = os.path.join(root, f)
            with open(path, 'rb') as fh:
                fh.seek(-1, os.SEEK_END)
                last = fh.read(1)
                if last != b'\n':
                    print(path)
                    fail = True
if fail:
    sys.exit(1)

