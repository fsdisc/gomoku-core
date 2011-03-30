<?php
require('../session.php');

$session = get('s');
$stop = false;
$data = query(sprintf('select * from %s where id = %s', stable('session'), quote($session)));
if (count($data) == 0) {
  print('Error: Invalid session!');
  $stop = true;
}
$userid = $data[0]['user_id'];

$gameId = get('g');
if (!$stop) {
  $data = query(sprintf('select * from %s where id = %s and joined is null and cancelled is null and finished is null', table('game'), quote($gameId)));
  if (count($data) == 0) {
    print('Error: Invalid game!');
    $stop = true;
  }
}

if (!$stop) {
  $playFirst = (strlen($data[0]['first_player']) > 0);
  if ($playFirst) {
    execute(sprintf('update %s set second_player = %s, joined = sysdate() where id = %s', table('game'), quote($userid), quote($gameId)));
  } else {
    execute(sprintf('update %s set first_player = %s, joined = sysdate() where id = %s', table('game'), quote($userid), quote($gameId)));
  }
  print('Success');
}

?>