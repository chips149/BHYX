# BHYX

《辟火英雄》是一款基于Unity开发的3D塔防Rougelike类游戏，项目原由PlayMaker全量重构为纯C#实现，剔除可视化脚本依赖。现已做完游戏全流程，已拿到国家版号待上线。

## 项目定位

由于本项目涉及到上线TapTap,所以本人在开发的过程中尝试按照完整小游戏的结构进行工程化拆分，目标包括：

\-构建一个可持续拓展的战斗系统

\-使用模块化方式进行管理游戏核心逻辑

\-使用配置和数据驱动怪物生成与关卡推进

\-通过卡牌机制实现玩家成长

\-通过存档系统支持恢复之前游戏进度

\-在功能实现以外，关注代码的可拓展性和维护性以及性能开销

## 技术栈

引擎：Unity 2022.3.62f3c1

语言：C#

项目类型：3D塔防RougeLike游戏

异步方案：UniTask

动画实现：DOTween/DOTween Pro

UI：Unity UI/TextMeshPro

架构设计：模块管理、状态机、计时器、数据驱动

数据存储：JsonUtility+本地JSON存档

## 核心玩法

进入战斗→初始化关卡数据、玩家、敌人→按波次生成敌人→玩家攻击并击败敌人→完成当前关卡→选择卡牌强化实力→进入下一关/切换无尽模式

## 项目结构

Assets/

├── Framework/                  # 通用框架代码

│   ├── BehaviorTree/           # 行为树相关

│   ├── StateMachine/           # 状态机

│   ├── Timer/                  # 计时器

│   ├── ModulesManager.cs       # 模块管理器

│   └── GlobalUpdate.cs         # 全局更新管理

│

├── Scenes/                     # Unity 场景

│   ├── MainMenuScene.unity     # 主菜单场景

│   ├── BootScene.unity         # 引导/开场场景

│   └── GameScene.unity         # 游戏战斗场景

│

├── Script/

│   ├── Battle/                 # 战斗相关逻辑

│   │   ├── BattleManager.cs    # 战斗管理

│   │   ├── GameInitializer.cs  # 游戏初始化

│   │   ├── EnemyManager.cs     # 敌人管理

│   │   ├── PlayerManager.cs    # 玩家管理

│   │   ├── SpawnMonsterHandler.cs # 怪物刷波

│   │   ├── Enemy/              # 敌人类型

│   │   ├── Player/             # 玩家、攻击、子弹、法宝

│   │   ├── Buff/               # Buff 系统

│   │   └── VFX/                # 战斗特效

│   │

│   ├── Card/                   # 卡牌系统

│   │   ├── CardHandler.cs      # 卡牌注册与随机

│   │   └── Instance/           # 具体卡牌实现

│   │

│   ├── Systems/                # 游戏系统

│   │   ├── CoinSystem.cs       # 金币系统

│   │   ├── MagicWeaponLevelUpSystem.cs

│   │   ├── SoundManager.cs

│   │   └── SaveSystem/         # 存档系统

│   │

│   └── UI/                     # UI 界面逻辑

│       ├── MainScene/          # 主菜单 UI

│       ├── Game/               # 游戏内 UI

│       ├── Card/               # 卡牌 UI

│       ├── MagicWeapon/        # 法宝 UI

│       └── Boot/               # 引导 UI

│

├── Plugins/                    # 第三方插件

└── Resources/                  # 运行时加载资源

## 核心功能

### 1.战斗管理系统

进入游戏后，由**GameInitializer统一管理战斗流程**，负责初始化存档与战斗模块。

设计目标：

\-降低场景对象之间的直接依赖

\-统一管理战斗生命周期

\-将战斗流程从具体的角色和UI抽离

\-便于后续拓展、暂停、结算等流程

实现要点：

\-通过ModulesManager获取并且控制PlayerManager和EnemyManager初始化

\-通过GlobalUpdate管理而非用MonoBehaviour进行帧更新

\-使用GameState保存当前战斗运行状态和事件回调

\-在战斗结束时同意释放相关模块

### 2.玩家攻击系统

玩家基础逻辑**由PlayerBase及相关攻击处理类**实现，包括瞄准、攻击冷却等逻辑。

系统特点：

\-鼠标点击进行瞄准

\-攻击间隔控制

\-子弹上限限制

\-子弹自动恢复

\-UI实时更新子弹数量

\-支持卡牌提升玩家属性

技术思考：

\-将攻击处理和瞄准处理抽离成两个独立类，减少了PlayerBase的职责

\-玩家属性集中储存，方便卡牌系统统一修改

\-为不同的法宝角色预留继承拓展空间，方便后续更新新的法宝

\-使用冷却计时器代替频繁创建协程，降低了运行时的开销

### 3.怪物生成系统

怪物生成统一由SpawnMonsterHandler控制，支持按照关卡、波次、阵型生成怪物。

系统特点：

\-支持多个刷怪点

\-支持不同难度波次按关卡读取怪物配置

\-支持无尽模式下的动态波次生成

\-支持随关卡推进而提升怪物的生命值与移速

工程亮点：

\-将刷怪机制与敌人行为逻辑分离，方便后期单独维护

\-使用SCV配置控制关卡节奏，方便策划修改战斗体验

\-怪物预制体通过ID映射加载，方便后续增加新的敌人

\-无尽模式中通过轮次计算波数和怪物强度，实现简单的动态成长

### 4.卡牌系统

卡牌系统由C#特性和反射机制进行自动注册，每张卡片只需继承CardData并添加CardPropertyAttribute,即可被系统扫描并注册到卡牌池

