# NetDeepL

.NET Standard wrapper for the [DeepL API v2](www.deepl.com/docs-api)

## NetDeepL.TranslationWorker

very basic console app that can translate excel spreadsheets (.xlsx)

* place .xlsx workbooks in same folder as application
* run application once to generate config file template if not already present
* fill in your api key, one source language and desired target languages
* application will loop through all .xlsx files, each worksheet, and translate each cell into all languages specified in config file
* per langage a new sheet will be created
* if sheets with `_DeepL_` are detected, tool will suggest to delete them

### Features

* doesn't translate hidden Excel rows or columns but only copies them into translated sheets
* doesn't create copies of empty sheets
* if connection to DeepL API is interrupted or lost, up to 3 retries are attempted for every cell that is translated
