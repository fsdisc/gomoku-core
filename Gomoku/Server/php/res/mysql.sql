CREATE TABLE `game_users` (
`id` VARCHAR( 16 ) NOT NULL ,
`username` VARCHAR( 16 ) NOT NULL ,
`password` VARCHAR( 255 ) NOT NULL ,
PRIMARY KEY ( `id` )
);

CREATE TABLE `game_session` (
`id` VARCHAR( 16 ) NOT NULL ,
`user_id` VARCHAR( 16 ) NOT NULL ,
`created` DATETIME NOT NULL ,
PRIMARY KEY ( `id` )
);

CREATE TABLE `gomoku_game` (
`id` VARCHAR( 16 ) NOT NULL ,
`first_player` VARCHAR( 16 ) NOT NULL ,
`second_player` VARCHAR( 16 ) NOT NULL ,
`created` DATETIME NOT NULL ,
`joined` DATETIME NULL ,
`cancelled` DATETIME NULL ,
`finished` DATETIME NULL ,
`victory` INT NOT NULL ,
`board_width` INT NOT NULL,
`board_height` INT NOT NULL,
PRIMARY KEY ( `id` )
);

CREATE TABLE `gomoku_move` (
`id` VARCHAR( 16 ) NOT NULL ,
`game_id` VARCHAR( 16 ) NOT NULL ,
`step` INT NOT NULL ,
`row` INT NOT NULL ,
`column` INT NOT NULL ,
`piece` INT NOT NULL ,
PRIMARY KEY ( `id` )
);
