﻿CREATE TABLE history_apartment_prices (
id_apartment INTEGER NOT NULL,
date_update_price TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP, --YYYY-MM-DD HH:MM:SS
price INTEGER NOT NULL DEFAULT 0
)