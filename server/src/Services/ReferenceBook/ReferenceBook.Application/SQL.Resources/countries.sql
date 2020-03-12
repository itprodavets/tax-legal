﻿SELECT
    c."Id",
    c."Name",
    c."Alpha2Code",
    c."Alpha3Code",
    c."NumericCode",
    c."Region",
    c."SubRegion"
FROM db.public."Countries" AS c
ORDER BY c."Name"