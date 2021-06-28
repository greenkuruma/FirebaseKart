# FirebaseKart

## 概要
Unityゲーム プログラミング・バイブル 2nd Generation
`テーマNo17 Firebaseを活用した非リアルタイムネットワークゲーム`
のサンプルプロジェクトです。

## 重要なお知らせ
【2021/06/28追記】
493ページでダウンロードする`google-services.json`ですが、初回DL時だと"firebase_url"の値が設定されていない事があるようです。

例
```
{
  "project_info": {
    "project_number": "xxxxxxxxxxxx",
    "firebase_url": "https://{project名からつくられた文字列}-rtdb.firebaseio.com", ←これが無い
    "project_id": "{project名からつくられた文字列}",
    "storage_bucket": "{project名からつくられた文字列}.appspot.com"
  },
  "client": [
```

その場合の対応策として

- プロジェクトの設定から`google-services.json`を再度DL
- 直接"firebase_url"を追記する。RealtimeDatabaseのデータページに載ってる"https://{project名からつくられた文字列}-rtdb.firebaseio.com"をコピペ

のいずれかの方法でご対応いただければ幸いです。
