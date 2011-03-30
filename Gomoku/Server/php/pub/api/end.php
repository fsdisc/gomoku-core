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
$finished = get('f') == '1' ? true : false;
$victory = get('v');

if (!$stop) {
  $data = query(sprintf('select * from %s where id = %s', table('game'), quote($gameId)));
  if (count($data) == 0) {
    print('Error: Invalid game!');
    $stop = true;
  } else {
    if (strlen($data[0]['finished']) > 0 || strlen($data[0]['cancelled']) > 0) {
      print('Success');
      $stop = true;
    }
  }
}

if (!$stop) {
  if ($finished) {
    execute(sprintf('update %s set finished = sysdate(), victory = %s where id = %s', table('game'), $victory, quote($gameId)));
  } else {
    execute(sprintf('update %s set cancelled = sysdate() where id = %s', table('game'), quote($gameId)));
  }
  print('Success');
}

?>