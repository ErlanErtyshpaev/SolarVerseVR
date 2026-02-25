# Technical Documentation — SolarVerse VR

## 1) Описание
SolarVerse VR — VR-симулятор Солнечной системы для Meta Quest. Пользователь перемещается между планетами и ведёт диалог с AI-гидом в UI-панели, получая объяснения в контексте выбранной планеты.

## 2) Архитектура решения
### Компоненты
- **VR Client (Unity):** сцена Солнечной системы, XR-управление, взаимодействие с планетами, UI-панель.
- **AI Layer (внутри Unity):** сбор контекста (выбранная планета + параметры), запрос в OpenAI API, обработка ответа.
- **UI Layer:** поле ввода, область ответа, кнопки отправки.

### Data Flow
1) Пользователь кликает по планете в VR  
2) Приложение получает `planet_name` и параметры планеты  
3) Формируется промпт/контекст (planet context + user message)  
4) Запрос отправляется в OpenAI API  
5) Ответ возвращается и отображается в UI-панели

## 3) Стек технологий
- **Unity 6**
- **XR Interaction Toolkit**
- **OpenXR**
- **Meta Quest**
- **UI:** Canvas / TextMeshPro
- **AI:** OpenAI API (LLM)

## 4) ИИ: используемый подход
- **LLM (Large Language Model)** для диалога и объяснений.
- **Контекстный промптинг:** ответы формируются на основе выбранной планеты и её параметров.
- (План развития) **RAG** на базе проверенных источников (NASA/ESA) для повышения точности.

## 5) Запуск и демонстрация (VR)
### Требования
- Unity 6
- XR Interaction Toolkit, OpenXR, Input System
- Meta Quest + Developer Mode
- OpenAI API Key

### Сборка на Quest
1) `File → Build Settings → Android → Switch Platform`
2) `Project Settings → XR Plug-in Management (Android) → OpenXR`
3) Подключить Quest по USB
4) `Build And Run`

### Демо-сценарий
См. файл `DEMO_GUIDE.md`.

## 6) Ограничения MVP
- MVP предназначен для локальной демонстрации.
- Точность ответов зависит от промптов и контекста (в перспективе — RAG и контроль источников).