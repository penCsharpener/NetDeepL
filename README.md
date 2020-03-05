# NetDeepL

.NET Standard wrapper for the [DeepL API v2](www.deepl.com/docs-api)

## NetDeepL.TranslationWorker

very basic console app that can translate excel spreadsheets (.xlsx)

* please .xlsx workbooks in same folder as application
* run application once to generate config file template
* fill in your api key, one source language and desired target languages
* application will loop through all .xlsx files, each workbook, and translate each cell into all languages specified in config file

