# Cursor AI Music Workshop - Mini Drum

這是你在工作坊中要使用的 Unity 專案基底，用來示範：

- 使用 Unity 建立簡單的互動「迷你鼓機」
- 透過 Git / GitHub / Fork 進行版本控制
- 練習「原子化 commit」的習慣與流程

---

## 1. 事前必備軟體與安裝連結

請在課前盡量完成以下安裝與檢查：

### 1. Git（版本控制核心引擎）

- **檢查方式：**
  - Windows：按 `Win + R` → 輸入 `cmd` → 在視窗中輸入 `git --version`。
  - Mac：打開 Terminal，輸入 `git --version`。
  - 若有出現版本號，即代表已安裝。
- **下載連結：**
  - Windows：[https://git-scm.com/download/win](https://git-scm.com/download/win)
  - Mac：通常內建，若無可透過 [https://git-scm.com/download/mac](https://git-scm.com/download/mac) 下載。

### 2. Cursor（AI 程式碼編輯器）

- **下載連結：** [https://cursor.com/](https://cursor.com/)
- **說明：** 本次實作 AI 協作與自動補完程式碼的核心工具。

### 3. Fork（Git 圖形化使用者介面）

- **下載連結：** [https://git-fork.com/](https://git-fork.com/)
- **說明：** 協助以視覺化方式操作「原子化提交」與分支管理。

### 4. Unity Hub（遊戲開發環境）

- **下載連結：** 請依提供的 Unity Hub 連結或前往 [Unity 官網](https://unity.com/) 下載。
- **注意：** 需登入才可下載（可用 Google 或 Apple 帳號快速登入）。若環境安裝耗時，當天亦有 Web 開發備案。

---

## 2. Unity 版本與專案開啟注意事項

- **請安裝的 Unity 版本：** `2023.2.22f1`
  - 可從 Unity 官方版本存檔頁面下載：[https://unity.com/releases/editor/archive](https://unity.com/releases/editor/archive)
  - 在 Unity Hub 中加入並安裝 `2023.2.22f1`。

- **請記住 Unity 專案的路徑**，等下要用 **Cursor** 開啟這個專案資料夾進行開發。  
  建議建立一個固定資料夾，例如：`C:\Users\<你的帳號>\Projects\Cursor-AI-Music-Workshop`。

---

## 3. 專案內容簡介

- **`Assets/Resources/Audio/`**
  - 基本鼓聲檔（Kick / Snare / ClosedHiHat）與其他鼓聲檔（Tom, Crash...）
- **`Assets/Scripts/MiniDrumManager.cs`**
  - 簡單的迷你鼓機管理腳本
  - 預設按鍵：
    - `Space`：Kick 大鼓
    - `J`：Snare 小鼓
    - `K`：Closed Hi-Hat 開合拔（閉合狀態）
  - 執行時會自動生成 3 個方塊（左中右），並在擊鼓時做縮放動畫

---

## 4. 學員當天流程（先用 Fork clone 講師 repo，再改 remote 推到自己 GitHub）

講師的 GitHub 專案連結：[https://github.com/otaku881221/Cursor-AI-Music-Workshop](https://github.com/otaku881221/Cursor-AI-Music-Workshop)

### 4-1. 使用 Fork 從講師 repo clone 專案

1. 開啟 **Fork** 應用程式。
2. 在 Fork 上方選單點選 **「File → Clone…」**（或介面中的 Clone 按鈕）。
3. 在「Repository URL」欄位貼上講師的 repo 連結：  
   `https://github.com/otaku881221/Cursor-AI-Music-Workshop.git`
4. 選擇要放專案的本機路徑（例如 `C:\Users\<你>\Projects\Cursor-AI-Music-Workshop`）。
5. 按下 **Clone**。
6. 完成後，Fork 會在左側顯示這個專案，你也可以在檔案總管中看到完整專案內容。

> 此時專案的 remote `origin` 仍指向「講師的 repo」，你沒有權限直接 push。  
> 接下來我們會在 GitHub 建立你自己的 repo，並把 remote 改成你自己的網址。

### 4-2. 在 GitHub 建立自己的空白 repo

1. 到 [https://github.com/](https://github.com/) 登入自己的 GitHub 帳號。
2. 右上角按下 `+` → 選擇 **「New repository」**。
3. 輸入 repo 名稱，例如：`Cursor-AI-Music-Workshop` 或你喜歡的名稱。
4. 建議先不要勾選「Initialize this repository with a README」等初始化選項（保持空白 repo 亦可）。
5. 按下 **Create repository**。
6. 在新建立的 repo 頁面，按 **「Code」** 按鈕，複製 HTTPS 連結，例如：  
   `https://github.com/<你的帳號>/Cursor-AI-Music-Workshop.git`

### 4-3. 在 Fork 中修改 remote 指向自己的 GitHub repo

1. 回到 **Fork**，選擇剛剛 clone 下來的專案。
2. 在上方工具列或側邊，找到並點選 **「Remotes」**（或右鍵專案名稱 → `Manage Remotes…`）。
3. 在列表中會看到一個 `origin`，目前的 URL 是講師的 repo。
4. 點選 `origin` → 按 **Edit**：
   - 將 URL 改成你剛才在 GitHub 建立 repo 的網址，例如：  
     `https://github.com/<你的帳號>/Cursor-AI-Music-Workshop.git`
   - 按 **Save** 儲存。
5. 完成後，這個本機專案的 `origin` remote 已經指向「你自己的 GitHub repo」，之後的 Push 都會推到你自己的帳號，不會影響講師原始專案。

### 4-4. 第一次 Push 到自己的 GitHub

1. 在 Fork 中確認目前是否有未提交的變更（第一次通常是跟講師專案一樣，不需要變更也可以先推上去）。
2. 在 Fork 右上角按下 **「Push」**。
3. 如果出現確認視窗，確定：
   - Remote：`origin`（即你自己的 repo）
   - Branch：`main`（或目前的預設分支）
4. 按下 **Push**。完成後，回到 GitHub 頁面重新整理，即可看到專案內容已出現在你自己的帳號底下。

> 之後的流程都是：在本機修改 → 用 Fork 做原子化 commit → 用 Fork 的 Push 推回「自己」的 GitHub repo。

---

## 5. 建議練習任務（用 Cursor 下指令實作）

以下任務請**在 Cursor 裡用對話下指令**完成，練習如何把需求講清楚、一次只請 AI 做一小步，再搭配 Fork 做原子化 commit。

1. **新增 Cowbell（牛鈴）**
   - 試著對 Cursor 下指令，例如：「請在 MiniDrumManager 裡新增 Cowbell，音檔在 Assets/Resources/Audio/Cowbell.wav，用 L 鍵觸發，並像 Kick/Snare/ClosedHiHat 一樣有可視方塊和縮放動畫。」
   - 若一次改太多，可拆成多句指令，例如先「只加 Cowbell 的按鍵與音效」，再「加 Cowbell 的方塊與顏色」。

2. **為每個鼓方塊加上文字 Label**
   - 對 Cursor 下指令，請它為每個鼓方塊加上名稱（Kick / Snare / Closed Hi-Hat / Cowbell 等），例如用 TextMeshPro 或 3D Text，並說明要顯示在方塊附近或畫面上。

3. **加入簡單節拍器（Metronome）**
   - 對 Cursor 下指令，例如：「請加一個節拍器，用現有音檔（例如 ClosedHiHat）當 tick 聲，可設定 BPM，並在畫面上顯示目前 BPM，用按鍵增加/減少。」（不需額外節拍器音檔。）

4. **加入簡單 UI**
   - 對 Cursor 下指令，請它在畫面上顯示按鍵說明（Space=Kick、J=Snare…），或擊鼓時的高亮提示，並說明你希望擺在哪裡、怎麼呈現。

> 重點：**一次只請 Cursor 做一小件事**，做完就到 Fork 做一次 commit，再下一個指令。這樣既能練「怎麼下指令」，也能練原子化 commit。

---

## 6. 用 Cursor 下指令 ＋ 原子化 commit 的流程（簡版）

以下用「新增 Cowbell（牛鈴）」當例子，示範：**對 Cursor 下一次小指令 → 檢查 → 在 Fork 裡 commit → 再下一個小指令**。

1. **在 Cursor 下一次「小」指令**  
   例如：「請在 MiniDrumManager 裡只先加 Cowbell 的按鍵 L 和音效播放，先不要加方塊。」  
   讓 AI 只做這一件事。

2. **檢查 Cursor 的改動**  
   在 Fork 的 Changes 區塊看改了哪些檔案、哪些行，確認符合預期。

3. **在 Fork 裡 Stage 並 Commit**  
   勾選這次改動的檔案，寫一句清楚的 commit 訊息，例如：`feat: add cowbell key and sound`，按下 **Commit**。

4. **再對 Cursor 下一個小指令**  
   例如：「請幫 Cowbell 加上可視方塊和顏色，和 Kick/Snare 一樣。」  
   做完後同樣到 Fork 檢查 → Stage → Commit，例如：`feat: add cowbell visual cube`。

5. **重複「下指令 → 檢查 → commit」**  
   之後若要加 Cowbell 的 Label、調顏色等，也是一次一個小指令、一個 commit。  
   例如：`feat: add cowbell label text`。

6. **Push 到自己的 GitHub**  
   在 Fork 右上角按 **Push**，把這串 commit 推到你自己的 repo。

> 重點：**練習把需求拆小、用一句話對 Cursor 下指令**，每完成一小步就 commit，方便回顧與除錯。

---

## 7. 講師示範時可以涵蓋的主題

- 使用 Fork clone 講師 repo、修改 remote 指向自己的 GitHub。
- `.gitignore` 在 Unity 專案中的重要性（避免把 `Library/` 等巨大資料夾推上雲端）。
- 在 Fork 中實際操作原子化 commit（一步一步新增某個小功能）。
- 透過 Fork 的歷史視圖觀察 commit 記錄。

如有額外進階內容（例如 Pull Request、Code Review 流程），可依實際課程時間再補充。  
歡迎依照講師實際需求調整本說明文件內容。
