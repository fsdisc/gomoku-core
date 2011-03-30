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

if (!$stop) {
  $data = query(sprintf('select * from %s where joined is null and cancelled is null and finished is null', table('game'))); 
  $tag = '';
  for ($i = 0; $i < count($data); $i++) {
    $gameId = $data[$i]['id'];
    $first = $data[$i]['first_player'];
    $second = $data[$i]['second_player'];
    $width = $data[$i]['board_width'];
    $height = $data[$i]['board_height'];
    if (strlen($first) > 0) {
      $first = getUsername($first);
    }
    if (strlen($second) > 0) {
      $second = getUsername($second);
    }
    if (strlen($tag) > 0) {
      $tag .= "\r\n";
    }
    $tag .= $gameId . '|' . $first . '|' . $second . '|' . $width . '|' . $height;
  }
  print('Success: ' . $tag);
}

function getUsername($id) {
  $result = query(sprintf('select * from %s where id = %s', stable('users'), quote($id)));
  if (count($result) == 0) {
    return '';
  } else {
    return $result[0]['username'];
  }
}

?>