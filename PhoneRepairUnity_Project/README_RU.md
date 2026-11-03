# Phone Repair Simulator — Unity Android

Это стартовый Unity-проект для переноса игры из предоставленных HTML/JS файлов в Unity.
Содержит базовый экран и игровой цикл ремонта: выбор телефона, диагностика, разборка,
замена детали, сборка и тестирование. Это **перенос базовой механики**, а не полная
пиксельная копия браузерной версии.

## Открыть
1. Установи Unity Hub и Unity **2022.3 LTS** с Android Build Support (SDK/NDK + OpenJDK).
2. Открой папку проекта `PhoneRepairUnity`.
3. Unity автоматически создаст сцену `Assets/Scenes/Main.unity` при открытии через
   меню `Tools → Generate Main Scene` (или при запуске Editor bootstrap).
4. В `File → Build Settings` выбери Android, Switch Platform, затем Build APK.

## Облачная сборка через GitHub Actions
1. Создай пустой GitHub repository и загрузи всё содержимое папки проекта.
2. В репозитории открой Settings → Secrets and variables → Actions.
3. Добавь secret `UNITY_LICENSE` с содержимым лицензии Unity Personal.
4. Запусти вкладку Actions → `Build Android APK`.
5. После завершения скачай `PhoneRepair.apk` из Artifacts.

Для GitHub Actions используется GameCI. Для активации Unity Personal может понадобиться
получить license file через Unity Hub / GameCI согласно официальной инструкции:
https://game.ci/docs/github/activation

## Важно
- Проект создан как Unity-проект (C# + Unity UI), а не WebView.
- Для полного переноса всех деталей, анимаций, экономики и мини-игр из браузерной версии
  потребуется дальнейшая итерация по исходному `game.js`.
- Android package id: `com.demo.phonerepairsimulator`.
