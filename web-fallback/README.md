# Mini Drum - 網頁版備案

當 **Unity 無法使用或安裝耗時** 時，可改用此網頁版完成工作坊流程（Clone → 用 Cursor 下指令改程式 → Fork 原子化 commit → Push）。

## 使用方式

1. 用瀏覽器開啟 `index.html`（雙擊或右鍵 → 開啟方式 → 瀏覽器）。
2. 按鍵與 Unity 版相同：**Space** = 大鼓、**J** = 小鼓、**K** = 開合鈸。
3. 用 **Cursor** 開啟本專案資料夾，對 `web-fallback/` 裡的檔案下指令做修改、練習原子化 commit。

## 技術說明

- 單一 HTML 檔，無需伺服器，可直接開啟。
- 鼓聲由 Web Audio API 即時合成，不需額外音檔。
- 可依工作坊需求用 Cursor 擴充（例如新增按鍵、樣式、BPM 顯示等）。
