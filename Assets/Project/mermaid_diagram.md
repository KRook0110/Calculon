classDiagram
    class C0["HealthBarUI"]
    class C1["BattleInitializer"]
    class C2["GameOverHandle"]
    class C3["TutorialHandler"]
    class C4["LevelNameText"]
    class C5["LevelData"]
    class C6["LevelSectorReseter"]
    class C7["LevelSelector"]
    class C8["LevelPlatform"]
    class C9["PlatformLookup"]
    class C10["LevelUnlocker"]
    class C11["LevelState"]
    class C12["LevelSelectionAnimationHandler"]
    class C13["PlayButton"]
    class C14["ActivationDebugger"]
    class C15["Fraction"]
    <<struct>> C15
    class C16["ParallaxLayer"]
    class C17["SaveDataViewer"]
    class C18["GameSaveData"]
    class C19["SaveSystem"]
    class C20["Singleton<T>"]
    class C21["SmoothCameraScroller"]
    class C22["UIFade"]
    class C23["ValueRange"]
    <<struct>> C23
    class C24["ChoicesBlockerHandler"]
    class C25["ChoiceData"]
    class C26["MultipleChoiceQuestion"]
    class C27["MultipleChoicesHandler"]
    class C28["MutlipleChoiceCooldown"]
    class C29["IDamageable"]
    <<interface>> C29
    class C30["Enemy"]
    class C31["DamageContext"]
    <<struct>> C31
    class C32["IProjectile"]
    <<interface>> C32
    class C33["RangedEnemy"]
    class C34["RangedEnemyProjectile"]
    class C35["SwarmEnemySpawner"]
    class C36["BossEnemy"]
    class C37["SummoningInfo"]
    class C38["CustomBossEnemy"]
    class C39["BasicEnemy"]
    class C40["PracticeTarget"]
    class C41["FightCoordinator"]
    class C42["FightData"]
    class C43["CustomChoice"]
    class C44["CustomQuestion"]
    class C45["InitialGameData"]
    class C46["FinalizedGameData"]
    class C47["ReactUnityCommunication"]
    class C48["BattleReactHandler"]
    class C49["LevelSelectionReactHandler"]
    class C50["AOEVisualEffect"]
    class C51["QuestionGenerator"]
    class C52["StageInfo"]
    class C53["QuestionInfo"]
    class C54["QuestionStage"]
    class C55["AdditionStage"]
    class C56["AdditionLevel"]
    <<struct>> C56
    class C57["CustomStage"]
    class C58["DivisionStage"]
    class C59["DivisionLevel"]
    <<struct>> C59
    class C60["FractionAdditionStage"]
    class C61["FractionLevel"]
    <<struct>> C61
    class C62["FractionMultilpicationStage"]
    class C63["FractionMultiplicationLevel"]
    <<struct>> C63
    class C64["FractionSubtractionStage"]
    class C65["FractionLevel"]
    <<struct>> C65
    class C66["MultiplicationStage"]
    class C67["MultiplicationLevel"]
    <<struct>> C67
    class C68["SubtractionStage"]
    class C69["SubtractionLevel"]
    <<struct>> C69
    class C70["ParentCenteringTool"]
    class C71["ProjectileMapping"]
    class C72["Mapping"]
    <<struct>> C72
    class C73["AnimationMapping"]
    class C74["AnimationData"]
    <<struct>> C74
    class C75["Mapping"]
    <<struct>> C75
    class C76["PlayerProjectile"]
    class C77["MultiPlayerProjectile"]
    class C78["BasicPlayerProjectile"]
    class C79["AOEPlayerProjectile"]
    class C80["AOEExplosionEffectHandler"]
    class C81["PlayerEntity"]
    class C82["PlayerDamageInfo"]
    <<struct>> C82
    class C0 {
        -_playerEntity : PlayerEntity
        -_targetUI : Transform
        -RefreshHealthBar()
        -OnEnable()
        -Start()
        -Update()
    }
    class C1 {
        -_hasInitializedBattle : bool
        -OnEnable()
        -OnDisable()
        -InitializeBattle()
    }
    class C2 {
        -_winUI : CanvasGroup
        -_loseUI : CanvasGroup
        -_playerEntity : PlayerEntity
        -_battleSceneName : string
        -_levelSelectSceneName : string
        -hasEnded : bool
        +OnWin : Action
        +OnLose : Action
        -OnEnable()
        -OnDisable()
        -HandleWin()
        -HandleLose()
        +RestartGame()
        +GoToLevelManager()
    }
    class C3 {
        -frames : List~CanvasGroup~
        +OnTutorialComplete : Action
        -m_currentFrameIndex : int
        -Start()
        -InitializeFrames()
        +NextFrame()
        -UpdateFrameVisibility()
        -CompleteTutorial()
    }
    class C4 {
        -_text : TextMeshProUGUI
        -Awake()
        -Start()
        -UpdateLevelText()
        -OnEnable()
        -OnDisable()
    }
    class C5 {
        +levelName : string
        +nextLevels : List~LevelData~
        +unlockQuestionTypes : List~string~
        +hasTutorial : bool
        +resetLevelSelectionToDefault : bool
        +enemies : List~GameObject~
    }
    class C6 {
        +fallbackLevel : LevelData
        -Start()
    }
    class C7 {
        -_selectedLevel : LevelData
        -_startingSelectedLevel : LevelData
        +OnSelectLevel : Action~LevelData~
        +selectedLevel : LevelData
        -Awake()
        +InitializeLevel()
    }
    class C8 {
        -_level : LevelData
        +Level : LevelData
        +mainCharacterPivot : Transform
        -_currentState : LevelState.State
        -_spriteRenderers : SpriteRenderer
        -_uiImages : UnityEngine.UI.Image
        -Awake()
        +OnPointerClick()
        -Start()
        -Checks()
        -UpdateVisuals()
        -UnlockQuestionTypes()
    }
    class C9 {
        -_platformLookup : Dictionary~LevelData, LevelPlatform~
        -_previousLevels : Dictionary~LevelData, List~LevelData~~
        +RegisterPlatform()
        +GetPlatform()
        +GetPath()
        +TryGetPlatformBoundaries()
    }
    class C10 {
        -OnEnable()
        -OnDisable()
        -UnlockNextLevels()
    }
    class C11 {
        -_initialUnlockedLevels : LevelData
        -_unlockedLevels : SortedSet~string~
        -_completedLevels : SortedSet~string~
        +OnLevelUnlocked : Action~string~
        +OnLevelCompleted : Action~string~
        -Awake()
        -DebugUnlockedLevels()
        -DebugCompletedLevels()
        -UnlockInitialLevels()
        -LoadData()
        -SaveData()
        -OnDestroy()
        +UnlockLevel()
        +GetLevelState()
        +IsUnlocked()
        +CompleteLevel()
        +IsCompleted()
    }
    class C12 {
        -character : Transform
        -animator : Animator
        -totalDuration : float
        -animationReferenceDuration : float
        -jumpTrigger : string
        -jumpHeight : float
        -jumpCurve : AnimationCurve
        -delayBetweenJumps : float
        -startingPlatform : LevelPlatform
        -currentPlatform : LevelPlatform
        -_targetLevel : LevelData
        -_isMoving : bool
        -_isInitialized : bool
        -Start()
        -OnEnable()
        -OnDisable()
        -OnSelectedLevelHandle()
        -MoveToTargetRoutine()
        -JumpRoutine()
        +FadeOutCharacterRoutine()
    }
    class C13 {
        -_sceneName : string
        -_clickSound : AudioClip
        -_transitionDelay : float
        -_buttonRef : Button
        -_canvasGroup : CanvasGroup
        -Awake()
        -OnEnable()
        -OnDisable()
        -UpdateVisuals()
        -HandleButtonClick()
        -PlaySoundAndLoadScene()
    }
    class C14 {
        -OnEnable()
        -OnDisable()
        -Awake()
    }
    class C15 {
        +Numerator : int
        +Denominator : int
        +operator : bool
        +! : bool operator
        +Fraction()
        +Add()
        +Subtract()
        +Multiply()
        +Simplify()
        -GetGCD()
        +ToString()
        +Equals()
        +GetHashCode()
    }
    class C16 {
        -parallaxEffect : float
        -infiniteScroll : bool
        -_length : float
        -_startPos : float
        -_cam : Transform
        -Start()
        -CreateClones()
        -CopySpriteRenderer()
        -LateUpdate()
    }
    class C17 {
        -_savePath : string
        -_data : GameSaveData
        -OnValidate()
        -Awake()
        +RefreshData()
        +SaveToDisk()
        +OpenSaveFolder()
        +ClearSaveData()
    }
    class C18 {
        +unlockedLevels : List~string~
        +completedLevels : List~string~
        +currentElo : int
    }
    class C19 {
        +SavePath : string
        +Save()
        +Load()
    }
    class C20 {
        -m_instance : T
        +Instance : T
        +HasInstance : bool
        -Awake()
        -OnDestroy()
    }
    class C21 {
        -buttonSpeed : float
        -scrollWheelSensitivity : float
        -smoothTime : float
        -minX : float
        -maxX : float
        -_targetX : float
        -_currentVelocity : float
        -_horizontalInput : float
        -_hasCentredInitial : bool
        -Start()
        +OnScrollInput()
        -Update()
    }
    class C22 {
        +Fade()
    }
    class C23 {
        +min : float
        +max : float
        +ValueRange()
        +Random()
    }
    class C24 {
        -blockerUI : GameObject
        -_objectsBlocking : HashSet~GameObject~
        +blocked : bool
        +Block()
        +IsBlocking()
        +UnBlock()
        -RefreshBlock()
        -Start()
        -HandleCooldownBlocking()
        -HandleCooldownUnBlocking()
        -OnEnable()
        -OnDisable()
    }
    class C25 {
        +isCorrect : bool
        +choiceText : string
    }
    class C26 {
        +questionText : string
        +choices : ChoiceData
        +CreateQuestion()
    }
    class C27 {
        -questionTextObject : TextMeshProUGUI
        -choiceObjects : GameObject
        -_choiceTexts : List~TextMeshProUGUI~
        -_choiceButtons : List~Button~
        -_currentQuestion : MultipleChoiceQuestion
        -_currentStage : QuestionStage
        +OnAnswer : Action~bool, QuestionStage~
        +OnFinish : Action
        -Start()
        +Answer()
        -NextQuestion()
        -RefreshUI()
        -RefreshBlocker()
        -RefreshQuestion()
        -RefreshChoices()
    }
    class C28 {
        -wrongAnswerCooldown : float
        -cooldowns : SortedSet~float~
        +OnUsable : Action
        +OnCooldown : Action
        +AddCooldown()
        -WrongAnswerHandle()
        -OnEnable()
        -OnDisable()
        -Update()
        -RemovePastCooldowns()
    }
    class C29 {
        +Damage()
    }
    class C30 {
        +player : Transform
        -playerTag : string
        -maxHealth : int
        -health : int
        +OnDeath : Action~Enemy~
        +OnDamaged : Action~DamageContext~
        +Kill()
        +Damage()
        +CompareTo()
        +SetHP()
    }
    class C31 {
        +remainingHealth : float
        +damageDone : int
    }
    class C32 {
        +Target : Transform
        +OnHit : Action~GameObject~
    }
    class C33 {
        -_damage : int
        -_movementSpeed : float
        -_attackRange : float
        -_attackInterval : float
        -_attackBurstCount : float
        -_attackBurstInterval : float
        -_spread : float
        -projectile : GameObject
        -projectileOrigin : Transform
        -_isAttacking : bool
        -_lastAttack : float
        -Update()
        -AttackSequence()
    }
    class C34 {
        +Target : Transform
        -_damage : int
        -_projectileLifeTime : float
        -_speed : float
        -_turnSpeed : float
        -_hitDistance : float
        +OnHit : Action~GameObject~
        -_lifeTime : float
        -Update()
    }
    class C35 {
        -_enemySpawnType : GameObject
        -_spawnAmount : int
        -_rotationRange : ValueRange
        -Start()
        -SpawnEnemy()
    }
    class C36 {
        -_summoningInfos : SummoningInfo
        -_summoningAnimationName : string
        -_summoningAnimationDuration : float
        -_summoningDelay : float
        -_summoningAnimationNameHash : int
        -_isSummoning : bool
        -Awake()
        -Start()
        -IsAllowedToMove()
        -Summoning()
    }
    class C37 {
        +delay : float
        +summoningPrefab : Enemy
        +amount : int
    }
    class C38 {
        -Start()
    }
    class C39 {
        -_damage : int
        -_speed : float
        -_attackCooldown : float
        -_attackKnockbackDistance : float
        -_attackKnockbackDuration : float
        -_onlyMoveForwards : bool
        -_rotationTrackingSpeed : float
        -_trackingDelay : float
        -_attackDelay : float
        -_animator : Animator
        -_atkAnimationName : string
        -_isRunningAnimationParamName : string
        -_hurtAnimationName : string
        -_hurtStunTime : float
        -_hurtKnockbackDistance : float
        -_hurtKnockbackDuration : float
        -_idleAnimationName : string
        -_deathAnimationName : string
        -_lastAttack : float
        -_allowMove : bool
        -_spawnTime : float
        -_isHurting : bool
        -_touchedEntity : IDamageable
        -_isDead : bool
        -_isRunningAnimationParamHash : int
        -_atkAnimationNameHash : int
        -_hurtAnimationNameHash : int
        -_idleAnimationNameHash : int
        -_deathAnimationNameHash : int
        -playerEntity : PlayerEntity
        -Awake()
        -Start()
        -IsAllowedToMove()
        -Update()
        -HandleHurt()
        +Kill()
        -DeathRoutine()
        -HurtRoutine()
        -OnEnable()
        -OnDisable()
        -MoveForward()
        -RotationTracking()
        -MoveDirectly()
        -Attack()
        -AttackRoutine()
        -SelfKnockbackRoutine()
        -HandleCollisionEnter()
        -HandleCollisionExit()
        -OnCollisionEnter2D()
        -OnCollisionExit2D()
        -OnTriggerEnter2D()
        -OnTriggerExit2D()
    }
    class C41 {
        -_multipleChoicesHandler : MultipleChoicesHandler
        -_playerEntity : PlayerEntity
        -_spawnOrigin : Transform
        -_playerProjectilePrefab : GameObject
        +_enemySpawns : List~GameObject~
        +aliveEnemies : SortedSet~Enemy~
        -_currentEnemyIndex : int
        +OnFinish : Action
        +Initialize()
        -OnEnable()
        -OnDisable()
        -FindClosestEnemy()
        -AnswerHandle()
        -HandleEnemyDeath()
        -SpawnNextEnemy()
        +SpawnEnemy()
        +SpawnEnemy()
    }
    class C42 {
        +enemies : List~GameObject~
    }
    class C43 {
        +text : string
        +is_correct : bool
    }
    class C44 {
        +question : string
        +choices : CustomChoice
    }
    class C45 {
        +all_levels_unlocked : string
        +current_elo : int
        +questions : CustomQuestion
    }
    class C46 {
        +elo_gained : int
        +new_levels_unlocked : string
    }
    class C47 {
        +_latestMessage : TextMeshProUGUI
        -customEnemyPrefab : GameObject
        -Init()
        -Level()
        -Answer()
        -Finished()
        -Awake()
        +GiveInitialdata()
        +Finalized()
        +TestGiveInitialData()
        +SendInit()
        +SendLevel()
        +SendAnswer()
        +SendFinished()
    }
    class C48 {
        -_player : PlayerEntity
        -Start()
        -OnEnable()
        -OnDisable()
        -HandleAnswers()
        -HandleFinishAlive()
        -HandleFinishDead()
    }
    class C49 {
        -Start()
    }
    class C50 {
        -_spriteRenderer : SpriteRenderer
        -Awake()
        +Initialize()
        -AnimateExplosion()
    }
    class C51 {
        -_unlockedStages : List~StageInfo~
        -_availableStages : List~StageInfo~
        -_customGameStage : QuestionStage
        +customQuestions : List~MultipleChoiceQuestion~
        -_currentCustomQuestionIndex : int
        +currentElo : int
        -eloGain : int
        -eloLoss : int
        -Awake()
        -LoadElo()
        -SaveElo()
        +EnableQuestionType()
        +UpdateElo()
        +SetElo()
        +ClearCustomQuestions()
        +ResetCustomQuestions()
        +AddCustomQuestion()
        +GenerateQuestion()
        -GetRandomStage()
    }
    class C52 {
        +chance : float
        +stage : QuestionStage
    }
    class C53 {
        +stage : QuestionStage
        +question : MultipleChoiceQuestion
    }
    class C54 {
        +name : string
        +currentElo : int
        +GenerateQuestion()
    }
    class C55 {
        -offsets : int
        -Levels : AdditionLevel
        -OnValidate()
        +GenerateQuestion()
        -GenerateChoiceValues()
    }
    class C56 {
        +minElo : int
        +minA : int
        +maxA : int
        +minB : int
        +maxB : int
        +AdditionLevel()
    }
    class C57 {
        -OnValidate()
        +GenerateQuestion()
    }
    class C58 {
        -Levels : DivisionLevel
        -OnValidate()
        +GenerateQuestion()
        -GenerateChoiceValues()
    }
    class C59 {
        +minElo : int
        +divisorPool : int
        +quotientPool : int
        +DivisionLevel()
    }
    class C60 {
        -Levels : FractionLevel
        -OnValidate()
        +GenerateQuestion()
        -GenerateChoices()
    }
    class C61 {
        +minElo : int
        +maxNumerator : int
        +maxDenominator : int
        +forceSameDenominator : bool
        +FractionLevel()
    }
    class C62 {
        -Levels : FractionMultiplicationLevel
        -OnValidate()
        +GenerateQuestion()
        -GenerateChoices()
    }
    class C63 {
        +minElo : int
        +numeratorPool : int
        +denominatorPool : int
        +FractionMultiplicationLevel()
    }
    class C64 {
        -Levels : FractionLevel
        -OnValidate()
        +GenerateQuestion()
        -GenerateChoices()
    }
    class C65 {
        +minElo : int
        +maxNumerator : int
        +maxDenominator : int
        +forceSameDenominator : bool
        +FractionLevel()
    }
    class C66 {
        -Levels : MultiplicationLevel
        -OnValidate()
        +GenerateQuestion()
        -GenerateChoiceValues()
    }
    class C67 {
        +minElo : int
        +poolA : int
        +poolB : int
        +MultiplicationLevel()
    }
    class C68 {
        -offsets : int
        -Levels : SubtractionLevel
        -OnValidate()
        +GenerateQuestion()
        -GenerateChoiceValues()
    }
    class C69 {
        +minElo : int
        +minA : int
        +maxA : int
        +minB : int
        +maxB : int
        +SubtractionLevel()
    }
    class C70 {
        +CenterParentBetweenChildren()
        +ValidateCenterParentBetweenChildren()
    }
    class C71 {
        -mappings : List~Mapping~
        -m_mappingDict : Dictionary~string, PlayerProjectile~
        -Awake()
        -InitializeDictionary()
        +GetProjectilePrefab()
    }
    class C72 {
        +stageName : string
        +projectilePrefab : PlayerProjectile
    }
    class C73 {
        -mappings : List~Mapping~
        -m_mappingDict : Dictionary~PlayerProjectile.Type, AnimationData~
        -Awake()
        -InitializeDictionary()
        +GetAnimationData()
    }
    class C74 {
        +animationName : string
        +projectileSpawnDelay : float
    }
    class C75 {
        +projectileType : PlayerProjectile.Type
        +animationData : AnimationData
    }
    class C76 {
        +type : Type
        +target : Enemy
        +OnHit : Action~PlayerProjectile~
        -spawnSound : AudioClip
        -spawnSoundVolume : float
        -hitSound : AudioClip
        -hitSoundVolume : float
        -PlaySound2D()
    }
    class C77 {
        -_amount : int
        -_projectile : PlayerProjectile
        -_rotationRange : ValueRange
        -Start()
        -SpawnProjectile()
    }
    class C78 {
        -_turnSpeed : float
        -_moveSpeed : float
        -_killDistance : float
        -_damage : int
        -_lifetime : float
        -_trackingDelay : float
        -_spawnTime : float
        -_dpos : Vector3
        -_onDeathSequence : bool
        -Start()
        -OnDestroy()
        -UpdateDPos()
        -HandleLifetime()
        -Update()
        -HandleMovement()
        -HandleSidewayMovement()
        -HandleEnemyDetection()
        -ProjectileDeathSequence()
        +DamageHandle()
    }
    class C79 {
        -_aoeDamage : int
        -_aoeRadius : float
        -_damageDelay : float
        -_explosionVFXPrefab : GameObject
        +DamageHandle()
        -DamageDelayRoutine()
        -SpawnAOEVFX()
    }
    class C80 {
        -_destroyTime : float
        -Start()
        -DestroyRoutine()
    }
    class C81 {
        -projectileOrigin : Transform
        -_maxHealth : int
        -_currentHealth : int
        +isDead : bool
        -_animator : Animator
        -_idleAnimationName : string
        -_hurtAnimationName : string
        -_deathAnimationName : string
        -_audioSource : AudioSource
        -_attackSound : AudioClip
        -_damageSound : AudioClip
        -_idleHash : int
        -_hurtHash : int
        -_deathHash : int
        +OnDamage : Action~PlayerDamageInfo~
        +OnDie : Action
        -Awake()
        +Damage()
        +Attack()
        -DelayedSpawn()
        -Start()
    }
    class C82 {
        +maxHealth : int
        +remainingHealth : int
        +damageTaken : int
    }
    class MonoBehaviour["MonoBehaviour"]
    MonoBehaviour <|-- C0
    class Singleton["Singleton<BattleInitializer>"]
    Singleton <|-- C1
    Singleton <|-- C2
    Singleton <|-- C3
    MonoBehaviour <|-- C4
    class ScriptableObject["ScriptableObject"]
    ScriptableObject <|-- C5
    MonoBehaviour <|-- C6
    Singleton <|-- C7
    MonoBehaviour <|-- C8
    class IPointerClickHandler["IPointerClickHandler"]
    IPointerClickHandler <|.. C8
    Singleton <|-- C9
    MonoBehaviour <|-- C10
    Singleton <|-- C11
    Singleton <|-- C12
    MonoBehaviour <|-- C13
    MonoBehaviour <|-- C14
    MonoBehaviour <|-- C16
    MonoBehaviour <|-- C17
    MonoBehaviour <|-- C20
    MonoBehaviour <|-- C21
    Singleton <|-- C24
    Singleton <|-- C27
    Singleton <|-- C28
    MonoBehaviour <|-- C30
    class IComparable["IComparable<Enemy>"]
    IComparable <|.. C30
    C30 <|-- C33
    MonoBehaviour <|-- C34
    C32 <|.. C34
    C30 <|-- C35
    C39 <|-- C36
    C36 <|-- C38
    C30 <|-- C39
    C30 <|-- C40
    Singleton <|-- C41
    Singleton <|-- C47
    Singleton <|-- C48
    Singleton <|-- C49
    MonoBehaviour <|-- C50
    Singleton <|-- C51
    MonoBehaviour <|-- C54
    C54 <|-- C55
    C54 <|-- C57
    C54 <|-- C58
    C54 <|-- C60
    C54 <|-- C62
    C54 <|-- C64
    C54 <|-- C66
    C54 <|-- C68
    class Editor["Editor"]
    Editor <|-- C70
    Singleton <|-- C71
    Singleton <|-- C73
    MonoBehaviour <|-- C76
    C76 <|-- C77
    C76 <|-- C78
    C78 <|-- C79
    MonoBehaviour <|-- C80
    MonoBehaviour <|-- C81
    C29 <|.. C81
