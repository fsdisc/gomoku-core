<?php
require('../session.php');

$session = get('s');

execute(sprintf('delete from %s where id = %s', stable('session'), quote($session)));

print('Success');

?>