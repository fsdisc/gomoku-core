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
  $data = query(sprintf('select * from %s where id = %s and joined is not null and cancelled is null and finished is null', table('game'), quote($gameId)));
  if (count($data) == 0) {
    print('Error: Invalid game!');
    $stop = true;
  }
}

if (!$stop) {
  $data = query(sprintf('select * from %s where game_id = %s order by step desc limit 1', table('move'), quote($gameId)));
  if (count($data) == 0) {
    print('Success: ');
  } else {
    $row = $data[0]['row'];
    $column = $data[0]['column'];
    $piece = $data[0]['piece'];
    print('Success: ' . $piece . '|' . $row . '|' . $column);
  }
}

?>