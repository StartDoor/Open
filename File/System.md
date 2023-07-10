# System系统
>介绍System系统的功能

## System系统的作用
>实现游戏逻辑和处理数据

## 具体业务介绍

### ActionPropertySystem:
>状态属性系统,用于控制球员当前所处的动作状态

#### 常用方法:
> 1. RevertActionComponent(PlayerEntity playerEntity) : 用于重置当前的动作状态
> 2. GetPropPriority(PlayerEntity playerEntity, int propId): 通过查找在已激活的buff中的位置来获取花式技能优先级
> 3. ReplaceActionComponent(int originId, PlayerEntity playerEntity, MatchEntity matchEntity) 去替换角色接下来的要使用的状态组件

### AIChatSystem:
>AI交流系统,用于AI球员的互动

#### 常用方法:
> 1. TriggerCommonChat(ERobotChatEvent eventId, MatchEntity match, ERobotChatSelection selection, PlayerEntity player) AI发送emoji表情

### AnimationSystem:
>动画系统,有个内部类AnimatonContainer用来处理数据

#### 常用方法:
> 1. GetShootAnimation(MatchEntity match, PlayerEntity playerEntity, FixedVector3 pos, bool goal, bool directIn) 获取投篮的动画,包括篮球弧线,弹框次数,是否空心球,掉落的远近等
> 2. GetLayupAnimation(PlayerEntity playerEntity, FixedVector3 pos, bool goal, int switchValue) 获取上篮的动画,包括上篮方式和弹框次数
> 3. GetDunkAnimation(PlayerEntity playerEntity, bool goal) 获取灌篮动画
> 4. TryGetPlayerAnimation(int animComponentId) 根据动画组件id来获取球员动画
> 5. PlayerAnimationEvent TryGetPlayerAnimationEvent(int compoentId, EPlayerEvent type) 获取球员动画事件,一般与获取球员动画配合使用
> 6. SetAnimation(
            PlayerEntity playerEntity,
            int animComponentId,
            FixedNumber speed,
            FixedNumber normalizedTransitionDuration,
            bool forcePlay,
            bool resetYZero,
            List<PlayerAnimationScaleNode> scaleList,
            MatchEntity matchEntity,
            EAnimationID animId = EAnimationID.NewAnim) 
              设置动画,将位移与动画保持一致,找寻将要播放的动画,给动画赋值和绑定事件,最后返回最终播放的动画
> 7. UpdateEvent 更新事件,激活当前事件和对应的Buff,缓存下一个事件和使用的技能

### AttributeSystem:
>属性系统

#### 常用方法:
> 1. CalculateBaseAttribute(EPlayerAttribute attrType, PlayerEntity self, MatchEntity matchEntity, PlayerEntity opponent = null) 计算基础属性数值,根据球员属性类型和等级得到基本数值,再加上附加buff和附加百分比buff计算出加值,然后算出最终数据

### AudioSystem:
>声音系统

#### 常用方法:
> 1. TriggerPlayerAudio(MatchEntity matchEntity, AudioConfItem audioItem) 触发音效播发,当没有音效可以播放的时候随机播放,否则就在缓存中的音效中随机触发一个播放
> 2. Play(MatchEntity matchEntity, int id, EAudioType type, EAudioTargetType targetType = EAudioTargetType.None, int index = -1) 播放音效,输入音频类型和发出声音的目标

### BallSystem:
>篮球系统

#### 常用方法:
> 1. PredictPosition(MatchEntity matchEntity, FixedNumber time) 预期篮球位置
> 2. TryShoot(PlayerEntity playerEntity, MatchEntity matchEntity, bool usePhysicalSimulation, bool goal, FixedVector3 v) 投篮,判断投篮时机,是否假透,开启投篮buff,是否投进,是否开启物理模拟
> 3. Physical(PlayerEntity playerEntity, MatchEntity matchEntity, FixedVector3 startPosition, FixedVector3 velocity, FixedNumber attachTime, EPlayerState fromState, int holderId) 将篮球的状态切换为物理模拟状态
> 4. Attach(PlayerEntity playerEntity, MatchEntity matchEntity, EPlayerState fromStateId, int holderId) 将篮球状态切换至附着状态
> 5. TryResponseBall(PlayerEntity playerEntity, MatchEntity matchEntity) 反应球,对捡球,篮板,分球,扑球和二次篮板做出对应的反应

### BlockSystem:
>盖帽系统

#### 常用方法:
> 1. JustJump(PlayerEntity playerEntity, MatchEntity matchEntity, SDBlockActionItem actionItem) 盖帽失败，只是让人物起跳
> 2. NormalBlock(PlayerEntity playerEntity, MatchEntity matchEntity, out BlockResultInfo blockResultInfo) 盖帽，判断自己和对手的状态是否可以盖帽，设置盖帽时间，方向，距离，发送盖帽结果通知，设置篮球位置，判断是否使用技能
> 3. IncreateBlockData(PlayerEntity playerEntity, MatchEntity matchEntity) 记录盖帽数据
> 4. BlockBaseCheck(PlayerEntity playerEntity, MatchEntity matchEntity) 盖帽的LateUpdate基本检查
> 5. BlockSuccessCommon(PlayerEntity playerEntity, MatchEntity matchEntity, PlayerEntity shooter, bool checkStrongHit = true) 盖帽成功的通用流程

### BreakSystem
>突破系统

#### 常用方法:
> 1. Ready(PlayerEntity playerEntity, MatchEntity matchEntity) 突破准备，切换突破状态，选择突破准备动画
> 2. SelectBreakRealAnimId(PlayerEntity playerEntity, MatchEntity matchEntity, SDBreakActionItem actionItem) 根据不同的突破方式选择对应的突破动画
> 3. CalRealBreakDir(PlayerEntity playerEntity, int animId) 根据动画最终位置算左右突破方向
> 4. Real(PlayerEntity playerEntity, MatchEntity matchEntity) 根据输入角度判断突破方向

### BuffSystem
>buff系统

#### 常用方法:
> 1. TryEnableBuff(PlayerEntity playerEntity, MatchEntity matchEntity, EBuffTriggerType triggerType, int param1) 先清除所有已激活的buff，然后尝试去激活未激活的buff，激活叠加buff
> 2. Update(PlayerEntity playerEntity, MatchEntity matchEntity) 刷新buff，尝试去激活未激活的buff，尝试结束激活中的buff，激活状态维持类buff
> 3. TryDisableBuffByBuffExit(PlayerEntity playerEntity, MatchEntity matchEntity, List\<int\>  buffids) 在buff结束的时候关掉buff
> 4. TryDisableBuff(PlayerEntity playerEntity, MatchEntity match, EBuffExitType exitType, int param) 遍历激活的buff，尝试去关掉可以关掉的buff

### CelebrationSystem
>庆祝系统,用于战斗胜利回合后的庆祝表现

#### 常用方法:
> 1. public enum ECelebrationState 庆祝动作的枚举状态
> 2. SyncCelebration(MatchEntity matchEntity) 庆祝函数,庆祝系统的主要函数,遍历比赛中玩家,判断对方的庆祝状态是否可以进行互动,进行庆祝动作播放

### CheckSkillInputSystem
>检查技能输入系统,检测释放技能的输入,判断技能是否能够触发,有大量的判断条件函数

#### 常用方法:
> 1. ChangeRuleSkills 内部类,用于存储规矩技能(RuleSkills),挂载在每个球员身上
> 2. InitRuleSkills(PlayerEntity playerEntity) 初始化规则技能,获取球员身上的技能数量,遍历加入ChangeRuleSkills
> 3. GetBallReletion(PlayerEntity playerEntity, MatchEntity matchEntity) 获取篮球当前所处的状态,包括哪方持球,是否空位,是否正在传球等
> 4. TriggerMatchSkill(PlayerEntity playerEntity, MatchEntity matchEntity, int downKey) 触发技能,在update中调用,先判断当前所处状态,再遍历所能使用的技能,触发能使用的技能

### CheckSystem
>检测系统,只有一个检测捡球的方法

#### 常用方法:
> 1. CheckPickup(PlayerEntity playerEntity, MatchEntity matchEntity) 判断当前状态,获取捡球动画,判断时机和球是否可以被接触,返回是否可以捡球

### DebugSystem
>Debug系统,主要用于Editor中配合GM调试用

#### 常用方法:
> 1. DebugOptionData Debug系统中的内部类,用来记录Debug的可选数据,以及管理各选项对应的操作,例如增删buff,重置方向,得分偷球篮板特殊能力等
> 2. HandleGMInput(FrameBuffer.GM gm, MatchEntity matchEntity) 用来控制GM的输入,例如投球方式,是否MVP,战斗数据等

### DefenseSystem
>防守系统

#### 常用方法:
> 1. Update(MatchEntity matchEntity) 更新玩家当前防守状态,判断当前状态是否可进行防守,计算需要防守的人数,找出当前需要防守的对象
> 2. UpdateOpponent(PlayerEntity playerEntity, MatchEntity matchEntity) 更新对手状态,判断对手玩家是否在防守范围之内,更新盯防时间
> 3. IsDefence(PlayerEntity playerEntity, MatchEntity matchEntity) 是否可防御,回合没有结束且当前队伍状态不是进攻状态则返回true

### DistanceSystem
>距离系统

#### 常用方法:
> 1. CalculatePlayerEntityDis(PlayerEntity a, PlayerEntity b) 计算两个玩家之间的距离并记录在缓存中
> 2. FixedNumber CalculatePlayerEntitySqrtDis(PlayerEntity lhs, PlayerEntity rhs) 计算两个玩家之间的方向角度并记录在缓存中

### DistrictSystem
>区域系统

#### 常用方法:
> 1. AddDistrict(PlayerEntity playerEntity, MatchEntity matchEntity, int buffId, FixedNumber redius, FixedNumber componentId, FixedVector3 position) 添加一个圆型领域并激活特效
> 2. RemoveDistrict(PlayerEntity playerEntity, MatchEntity matchEntity) 删除玩家所处的特殊区域并删除特效

### EffectSystem
>特效系统

#### 常用方法:
> 1. Show(PlayerEntity playerEntity, EEffectType type, float endTime = -1.0f,int effectid = -1, int flag = -1, int flag2 = -1) 展示特效,常用方法
> 2. GetEffectItem(int effectId, MatchEntity matchEntity, PlayerEntity playerEntity, bool useCache = false) 获取特效物件

### EventSystem
>事件系统,用来记录一些事件,例如回放,游戏退出等

#### 常用方法:
> 1. SetSnapshotReplay(System.Action<bool> action) 设置快照回放
> 2. OnTriggerGameExit(bool waite) 在游戏退出时触发

### GivenupSystem
>投降系统

#### 常用方法:
> 1. SyncGivenUp(MatchEntity matchEntity, ref FrameBuffer.Frame inputFrame) 投降,投降系统的主要功能,先判断当前状态是否可以投降,随后遍历玩家列表,记录选择接受投降和反对的玩家,最后统计投降结果并返回

### InjectLogSystem
>注入日志系统,用于输出日志,常用系统

#### 常用方法:
> 1. Save(long playerPos, long season) 保持日志,并通过oss上传

### KeySystem
>按键系统,包括实际按钮和逻辑按钮,主要用来返回对按钮操作之后的状态

#### 常用方法:
> 1. RealKeyClicked(PlayerEntity playerEntity, ERealInputKey key) 判断按钮是否能被按下
> 2. IsYawTypeStop(int yaw) 判断输入的角偏移结束

### MaskSystem
>遮罩系统

#### 常用方法:
> 1. SetEffectMask(PlayerEntity playerEntity, EEffectCondition type) 设置特效遮罩
> 2. SetSkillMask(PlayerEntity playerEntity, int skillId) 获取技能id,将使用的技能id记录到玩家信息里

### MatchSystem
>比赛系统

#### 常用方法:
> 1. struct SpecialSkillStatisticsGroup 特殊技能统计组,用来记录特殊技能ID,使用次数和成功次数的结构体
> 2. public static pb.GamerPVPResultC2S ConvertGameResult(MatchEntity match) 统计比赛中的比分,是否投降,各类事件(比如盖帽次数,上篮次数等)数据,不同得分方式的数据,突破数据,防守数据,能量数据,将统计到的数据记录在GamerPVPResultC2S中,发给服务器
> 3. public static pb.GamerPVPResultOtherC2S ConvertGameResultOther(MatchEntity match) 统计比赛中的额外数据,一般是一些特殊事件的触发数据,,将统计到的数据记录在GGamerPVPResultOtherC2S中,发给服务器
> 4. SetEvent(MatchEntity matchEntity, EMatchEvent eventType, int playerId = -1, EAudioType audio = EAudioType.None) 设置玩家事件,将事件类型和玩家id记录在比赛事件中,且事件类型的音效不为空的话,就播放对应的音效
> 5. SetPlayerEvent(MatchEntity matchEntity, EMatchEvent eventType, int playerId) 设置npc事件,将事件类型和玩家id记录在比赛事件中

### MatchTimeSlowdownSystem
>比赛时间缓速系统,用来实现减缓时间的技能效果

#### 常用方法
> 1. UpdateMatchTime(MatchEntity matchEntity) 遍历玩家身上已激活的buff,如果有TimeSlowDown则按照配表数据缩放比赛时间timeScale

### MoveSystem
>移动系统

#### 常用方法
> 1. SetPlayerOnGround(PlayerEntity playerEntity) 保持玩家的移动方向movement的position和rotation不变,将玩家的移动方向movement的Y轴的角度偏移重置,将动画位置和方向置为默认状态,将玩家transform的X和Z轴不变,将Y轴置为0
> 2. SetPositionAndRotationImmediately(PlayerEntity playerEntity, FixedVector3 pos, FixedNumber angle, int stayFrame = 1) 首先设置球员移动的角度和位置,计算剩余要移动的角度和位置,然后根据位移和动画的数据更新玩家的方向和位置
> 3. UpdateAnimation(PlayerEntity playerEntity, MatchEntity matchEntity) 更新玩家动画,根据缩放系数和开始动画的时间获取动画当前阶段人物和球的位置,然后更加玩家是否处于"双手抱球"状态来设置玩家的位置和方向

### NumericalSystem
>数字系统,用于设置游戏中的定数,基础算法和游戏中常用的计算公式

#### 常用方法
> 1. GetSpeedMove(PlayerEntity playerEntity, MatchEntity matchEntity) 获取无球移动速度,根据玩家身上激活的buff计算移动速度
> 2. GetAnimSpeedMove(PlayerEntity playerEntity, MatchEntity matchEntity) 获取跑动动画速度,根据玩家的职业和缩放系数计算动画速度
> 3. CanBeBreakSway(PlayerEntity player, MatchEntity matchEntity) 计算可否被晃倒,根据玩家身上激活的buff和随机数计是否可以被晃倒
> 4. CalculateBreakSwayResult(PlayerEntity playerEntity, MatchEntity matchEntity, PlayerEntity oppoEntity) 计算玩家在防守突破被晃之后的结果,计算被晃倒和被晃失稳的几率,返回对应的状态

### PassSystem
>传球系统

#### 常用方法
> 1. CanCatch(PlayerEntity ctrl) 根据玩家当前状态判断是否能够抢断
> 2. FindBlockers(PlayerEntity fromPlayer, PlayerEntity toPlayer, MatchEntity matchEntity) 找出传球路径中的阻挡人,计算与其他玩家的距离,判断其他玩家的状态,得出传球路径是否在其他玩家的干扰半径中
> 3. GetCatcherPos(PlayerEntity target, BallEntity ballEntity, FixedVector3? renderPos = null) 获取阻挡人的位置

### PhysisSystem
>物理系统,用于记录模拟物理的算法

#### 常用方法
> 1. GetCollsionPriority(PlayerEntity playerEntity, MatchEntity matchEntity) 从玩家的状态机中获取当期状态玩家的碰撞数据
> 2. PreUpdate(MatchEntity matchEntity) 更新玩家的物理状态,根据玩家的碰撞半径计算玩家的碰撞状态和碰撞数据
> 3. PostUpdate(MatchEntity matchEntity) 更新玩家的物理状态,根据玩家的碰撞信息计算接下来玩家位置和方向,并且预测接下来可能发生的碰撞

### RandomSystem
>随机系统

#### 常用方法
> 1. Random(PlayerEntity playerEntity, int min, int max) 用玩家数据中runtimeProperty里的种子随机一个范围之内的值

### ReboundSystem
>篮板系统,用于记录篮板球相关算法

#### 常用方法
> 1. Check(PlayerEntity playerEntity, MatchEntity matchEntity, out FixedVector3 ballPosition, FixedNumber fireTime, bool justCheck = false) 判断篮板球是否成功,先判断当前是否处于特殊状态(新手引导中的篮板教学阶段和热身赛),然后判断球的状态是否可以被篮板球,再计算玩家的位置,篮板的高度,篮板的宽度,跳板时间,最后判断玩家是否跳板成功(跳板过早,跳板过晚,跳空篮板还是跳板成功)

### ReplaySytem
>回放系统,主要用来记录帧数据和输出log

#### 常用方法
> 1. SaveFrame( FrameBuffer.Frame playerInput, FrameBuffer buffer) 记录帧数据,判断当前帧数据是否已经被记录,未被记录就添加当前帧输入数据,遍历所有球员将有GM标记的玩家帧数据单独记录,其他的球员普通记录

### SectorSystem
>区域系统,主要用来计算干扰区数据变化

#### 常用方法
> 1. Update(MatchEntity matchEntity) 更新干扰区数据,选择持球人,将持球人干扰区内的玩家数据记录下了,随后遍历所有玩家清空他们的干扰区数据,重新计算他们的干扰区数据(干扰区面积,有多少人在干扰区内,玩家受到什么程度的干扰)

### ShootSystem
>投篮系统,用于记录与投篮相关的算法(不同投篮方式触发的事件,buff和动画)

#### 常用方法
> 1. SetScale(PlayerEntity playerEntity, MatchEntity matchEntity, int switchId, bool triggeredBasketScale) 设置投篮的缩放,这个方法会在投篮准备和篮球打框的时候各调用一次,在投篮准备的时候计算出手位置到打框位置的距离对投篮动画进行缩放,在篮球打框的时候计算玩家位置,出手位置和球员脚跟位置对投篮动画进行缩放
> 2. TryTriggerCameraEvent(MatchEntity matchEntity, PlayerEntity playerEntity, CameraView.EventCameraType camType) 尝试概率触发相机事件,只在非战斗校验包的情况下才执行,随机概率触发
> 3. UpdateBackboardBrokenEffect(MatchEntity matchEntity, PlayerEntity playerEntity) 篮板裂纹动画, 寻找篮板材质,根据篮板赔损程度更新篮板材质球

### ShowSystem
>展示系统,用于展示比赛中的各种动画

#### 常用方法
> 1. Show(MatchEntity matchEntity, PlayerEntity playerEntity) 播放玩家正在展示的动画,重置玩家位置和方向,将玩家位置重置到地面上
> 2. SetShowAnimRandom(MatchEntity matchEntity, PlayerEntity playerEntity, List<int> anims, PlayerFixedPos posInfo, bool remove) 从动画列表中随机播放一个
> 3. SetPlayerShowAnim(PlayerEntity playerEntity, MatchEntity matchEntity, int animId, FixedVector3? pos = null, FixedNumber? angle = null) 播放球员将要展示的动画,从ShowAnimConfig表中获取动画数据(位置,角度,是否抓球等),获取玩家当前状态,将动画组件赋值上当前动画并返回动画播放时间

### SkillSystem
>技能系统,用于设置并记录技能信息

#### 常用方法
> 1. SelectSkill(PlayerEntity playerEntity, MatchEntity matchEntity, ESkillSelect selectRule, int poolType, EPlayerState[] skillTypes, int inputKey, int passTargetId) 获取技能,如果是循环技能就只能使用当前轮到的技能,剔除掉没轮到的技能,否则根据玩家状态和技能类型选择可以执行的技能
> 2. VerifyChangeSkill(PlayerEntity playerEntity, MatchEntity matchEntity, ESkillSelect selectRule, PlayerSkill skill) 验证变化技能,记录下当前技能,获取技能按键输入,技能信息和技能目标,然后根据玩家状态和技能类型选择可以执行的技能

### SnapshotSystem
>快照系统

#### 常用方法
> 1. BeginCapture(ECaptureType type, MatchEntity matchEntity) 开始捕获,在除了跳球阶段,回放阶段和爬塔游戏模式之外的情况下,将当前帧记录为快照开启帧
> 2. ConfirmeSnapshot(MatchEntity matchEntity ) 更新快照,每一帧更新,处理捕捉的动画并放入_snapshots,先获取比赛数据中的快照开启帧比对当前帧,如果是当前帧则将捕获的精彩镜头放入_states,后获取比赛数据中的快照结束帧比对当前帧,如果是当前帧则结束捕捉将精彩事件放入_captureQueue,然后遍历玩家数据中的快照结束帧比对当前帧,如果是当前帧则结束捕捉将精彩事件放入_captureQueue
> 3. SetEvent(MatchEntity matchEntity, ESnapshotEvent eventType, int playerId = -1) 设置精彩事件,获取玩家的快照事件队列,将大于600帧的部分出列,将新的精彩事件入列

### StatisticsSystem
>统计系统

#### 常用方法
> 1. IncreaseSpecialSkillUseCount(PlayerEntity playerEntity) 增加特殊技能使用次数,获取玩家已使用过的特殊子技能列表和玩家正在使用的特殊子技能,如果列表中已经有了该技能的记录则记录次数加一,没有则增加当前子技能的记录,同理特殊主技能也是一样记录

### TrainingSystem
>训练系统

#### 常用方法
> 1. AddCheckDictByPlayer(EPlayerState ePlayerState, OnCheckFuncType checkType) 添加check类型到_checkPlayerActions
> 2. SetTrainSkill(MatchEntity matchEntity, int skillId, int trainId) 设置训练技能,筛选出当前练习对应的循环技能,记录循环技能中当前技能的主技能id,根据训练id获取并记录训练数据
> 3. CheckPlayerUseSkill(MatchEntity matchEntity, PlayerEntity playerEntity) 检测玩家使用技能,检测玩家正在使用的技能是不是设置好的训练技能
> 4. ShowResult(MatchEntity matchEntity, PlayerEntity playerEntity, bool success) 展示训练结果,按照成功与否播放不同的特效,并将训练数据记录