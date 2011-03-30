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
  $data = query(sprintf('select * from %s where id = %s', table('game'), quote($gameId)));
  if (count($data) == 0) {
    print('Error: Invalid game!');
    $stop = true;
  } else {
    $joined = strlen($data[0]['joined']) == 0 ? 0 : 1;
    $cancelled = strlen($data[0]['cancelled']) == 0 ? 0 : 1;
    $finished = strlen($data[0]['finished']) == 0 ? 0 : 1;
    $victory = $data[0]['victory'];
    print('Success: ' . $joined . '|' . $cancelled . '|' . $finished . '|' . $victory);
  }
}

?>