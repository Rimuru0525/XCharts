using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public enum AnimationType
    {
        /// <summary>
        /// デフォルト。内部で実際の状況に応じてアニメーション再生方式が選択されます。
        /// || デフォルト。内部で実際の状況に応じてアニメーション再生方式が選択されます。
        /// </summary>
        Default,
        /// <summary>
        /// アニメーションを左から右へ再生します。
        /// || アニメーションを左から右へ再生します。
        /// </summary>
        LeftToRight,
        /// <summary>
        /// アニメーションを下から上へ再生します。
        /// || アニメーションを下から上へ再生します。
        /// </summary>
        BottomToTop,
        /// <summary>
        /// アニメーションを内側から外側へ再生します。
        /// || アニメーションを内側から外側へ再生します。
        /// </summary>
        InsideOut,
        /// <summary>
        /// パスに沿ってアニメーションを再生します。折れ線グラフが左から右へ無秩序または折り返しがある場合、このモードを使用できます。
        /// || パスに沿ってアニメーションを再生します。折れ線グラフが左から右へ無秩序または折り返しがある場合、このモードを使用できます。
        /// </summary>
        AlongPath,
        /// <summary>
        /// アニメーションを時計回りに再生します。
        /// || アニメーションを時計回りに再生します。
        /// </summary>
        Clockwise,
    }

    public enum AnimationEasing
    {
        Linear,
    }

    /// <summary>
    /// シリーズ（データ系列）のアニメーション。fadeIn、fadeOut、change、addition、exchangeなどのアニメーションタイプに対応。
    /// || アニメーションコンポーネント。グラフのアニメーション再生を制御します。FadeIn（フェードイン）、FadeOut（フェードアウト）、Change（変更）、Addition（追加）、Interaction（インタラクション）、Exchange（入れ替え）の5種類のアニメーション表現をサポート。
    /// 対象によってSerieAnimation（シリーズアニメーション）とDataAnimation（データアニメーション）に分類されます。
    /// </summary>
    [System.Serializable]
    public class AnimationStyle : ChildComponent
    {
        [SerializeField] private bool m_Enable = true;
        [SerializeField] private AnimationType m_Type;
        [SerializeField] private AnimationEasing m_Easting;
        [SerializeField] private int m_Threshold = 2000;
        [SerializeField][Since("v3.4.0")] private bool m_UnscaledTime;
        [SerializeField][Since("v3.8.0")] private AnimationFadeIn m_FadeIn = new AnimationFadeIn();
        [SerializeField][Since("v3.8.0")] private AnimationFadeOut m_FadeOut = new AnimationFadeOut() { reverse = true };
        [SerializeField][Since("v3.8.0")] private AnimationChange m_Change = new AnimationChange() { duration = 500 };
        [SerializeField][Since("v3.8.0")] private AnimationAddition m_Addition = new AnimationAddition() { duration = 500 };
        [SerializeField][Since("v3.8.0")] private AnimationHiding m_Hiding = new AnimationHiding() { duration = 500 };
        [SerializeField][Since("v3.8.0")] private AnimationInteraction m_Interaction = new AnimationInteraction() { duration = 250 };
        [SerializeField][Since("v3.15.0")] private AnimationExchange m_Exchange = new AnimationExchange() { duration = 250 };

        [Obsolete("Use animation.fadeIn.delayFunction instead.", true)]
        public AnimationDelayFunction fadeInDelayFunction;
        [Obsolete("Use animation.fadeIn.durationFunction instead.", true)]
        public AnimationDurationFunction fadeInDurationFunction;
        [Obsolete("Use animation.fadeOut.delayFunction instead.", true)]
        public AnimationDelayFunction fadeOutDelayFunction;
        [Obsolete("Use animation.fadeOut.durationFunction instead.", true)]
        public AnimationDurationFunction fadeOutDurationFunction;
        [Obsolete("Use animation.fadeIn.OnAnimationEnd() instead.", true)]
        public Action fadeInFinishCallback { get; set; }
        [Obsolete("Use animation.fadeOut.OnAnimationEnd() instead.", true)]
        public Action fadeOutFinishCallback { get; set; }
        public AnimationStyleContext context = new AnimationStyleContext();

        /// <summary>
        /// アニメーション効果を有効にするかどうか。
        /// || アニメーション効果を有効にするかどうか.
        /// </summary>
        public bool enable { get { return m_Enable; } set { m_Enable = value; } }
        /// <summary>
        /// アニメーションの種類。
        /// || アニメーションの種類.
        /// </summary>
        public AnimationType type
        {
            get { return m_Type; }
            set
            {
                m_Type = value;
                if (m_Type != AnimationType.Default)
                {
                    context.type = m_Type;
                }
            }
        }
        /// <summary>
        /// アニメーションを有効にするグラフィック数の閾値。グラフィック数がこの閾値を超えるとアニメーションは無効になります。
        /// </summary>
        public int threshold { get { return m_Threshold; } set { m_Threshold = value; } }
        /// <summary>
        /// アニメーションの更新がTime.timeScaleに依存しないようにします。
        /// </summary>
        public bool unscaledTime { get { return m_UnscaledTime; } set { m_UnscaledTime = value; } }
        /// <summary>
        /// フェードインアニメーションの設定。
        /// </summary>
        public AnimationFadeIn fadeIn { get { return m_FadeIn; } }
        /// <summary>
        /// フェードアウトアニメーションの設定。
        /// </summary>
        public AnimationFadeOut fadeOut { get { return m_FadeOut; } }
        /// <summary>
        /// データ更新アニメーションの設定。
        /// </summary>
        public AnimationChange change { get { return m_Change; } }
        /// <summary>
        /// データ追加アニメーションの設定。
        /// </summary>
        public AnimationAddition addition { get { return m_Addition; } }
        /// <summary>
        /// データ非表示アニメーションの設定。
        /// </summary>
        public AnimationHiding hiding { get { return m_Hiding; } }
        /// <summary>
        /// インタラクションアニメーションの設定。
        /// </summary>
        public AnimationInteraction interaction { get { return m_Interaction; } }
        /// <summary>
        /// 交換アニメーションの設定。ソートされた棒グラフなどで有効です。
        /// </summary>
        public AnimationExchange exchange { get { return m_Exchange; } }

        private Vector3 m_LinePathLastPos;
        private List<AnimationInfo> m_Animations;
        private List<AnimationInfo> animations
        {
            get
            {
                if (m_Animations == null)
                {
                    m_Animations = new List<AnimationInfo>
                    {
                        m_FadeIn,
                        m_FadeOut,
                        m_Change,
                        m_Addition,
                        m_Hiding,
                        m_Exchange
                    };
                }
                return m_Animations;
            }
        }

        /// <summary>
        /// 現在アクティブなアニメーション。
        /// </summary>
        public AnimationInfo activedAnimation
        {
            get
            {
                foreach (var anim in animations)
                {
                    if (anim.context.start) return anim;
                }
                return null;
            }
        }

        /// <summary>
        /// フェードインアニメーションを開始します。
        /// </summary>
        public void FadeIn()
        {
            if (m_FadeOut.context.start) return;
            m_FadeIn.Start();
        }

        /// <summary>
        /// アクティブなアニメーションを再開します。
        /// </summary>
        public void Restart()
        {
            var anim = activedAnimation;
            Reset();
            if (anim != null)
            {
                anim.Start();
            }
        }

        /// <summary>
        /// フェードアウトアニメーションを開始します。
        /// </summary>
        public void FadeOut()
        {
            m_FadeOut.Start();
        }

        /// <summary>
        /// データ追加アニメーションを開始します。
        /// </summary>
        public void Addition()
        {
            if (!enable) return;
            if (!m_FadeIn.context.start && !m_FadeOut.context.start)
            {
                m_Addition.Start(false);
            }
        }

        /// <summary>
        /// すべてのアニメーションを一時停止します。
        /// </summary>
        public void Pause()
        {
            foreach (var anim in animations)
            {
                anim.Pause();
            }
        }

        /// <summary>
        /// すべてのアニメーションを再開します。
        /// </summary>
        public void Resume()
        {
            foreach (var anim in animations)
            {
                anim.Resume();
            }
        }

        /// <summary>
        /// すべてのアニメーションをリセットします。
        /// </summary>
        public void Reset()
        {
            foreach (var anim in animations)
            {
                anim.Reset();
            }
        }

        /// <summary>
        /// アニメーションの進行状況を初期化します。
        /// </summary>
        /// <param name="curr">現在の進捗</param>
        /// <param name="dest">目標の進捗</param>
        public void InitProgress(float curr, float dest)
        {
            var anim = activedAnimation;
            if (anim == null) return;
            var isAddedAnim = anim is AnimationAddition;
            if (IsSerieAnimation())
            {
                if (isAddedAnim)
                {
                    anim.Init(anim.context.currPointIndex, dest, (int)dest - 1);
                }
                else
                {
                    m_Addition.context.currPointIndex = (int)dest - 1;
                    anim.Init(curr, dest, (int)dest - 1);
                }
            }
            else
            {
                anim.Init(curr, dest, 0);
            }
        }

        /// <summary>
        /// アニメーションの進行状況を初期化します。
        /// </summary>
        /// <param name="paths">パスの座標点リスト</param>
        /// <param name="isY">Y軸かX軸か</param>
        public void InitProgress(List<Vector3> paths, bool isY)
        {
            if (paths.Count < 1) return;
            var anim = activedAnimation;
            if (anim == null)
            {
                m_Addition.context.currPointIndex = paths.Count - 1;
                return;
            }
            var isAddedAnim = anim is AnimationAddition;
            var startIndex = 0;
            if (isAddedAnim)
            {
                startIndex = anim.context.currPointIndex == paths.Count - 1 ?
                    paths.Count - 2 :
                    anim.context.currPointIndex;
                if (startIndex < 0 || startIndex >= paths.Count - 1) return;
            }
            else
            {
                m_Addition.context.currPointIndex = paths.Count - 1;
            }
            var sp = paths[startIndex];
            var ep = paths[paths.Count - 1];
            var currDetailProgress = isY ? sp.y : sp.x;
            var totalDetailProgress = isY ? ep.y : ep.x;
            if (context.type == AnimationType.AlongPath)
            {
                currDetailProgress = 0;
                totalDetailProgress = 0;
                var lp = sp;
                for (int i = 1; i < paths.Count; i++)
                {
                    var np = paths[i];
                    totalDetailProgress += Vector3.Distance(np, lp);
                    lp = np;
                    if (startIndex > 0 && i == startIndex)
                        currDetailProgress = totalDetailProgress;
                }
                m_LinePathLastPos = sp;
                context.currentPathDistance = 0;
            }
            if (sp == anim.context.currPoint && ep == anim.context.destPoint)
            {
                return;
            }

            if (anim.Init(currDetailProgress, totalDetailProgress, paths.Count - 1))
            {
                anim.context.currPoint = sp;
                anim.context.destPoint = ep;
            }
        }

        public bool IsEnd()
        {
            foreach (var animation in animations)
            {
                if (animation.context.start)
                    return animation.context.end;
            }
            return m_FadeIn.context.end;
        }


        public bool IsFinish()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return true;
#endif
            if (!m_Enable)
                return true;
            var animation = activedAnimation;
            if (animation != null && animation.context.end)
            {
                return true;
            }
            if (IsSerieAnimation())
            {
                if (m_FadeOut.context.start)
                {
                    return m_FadeOut.context.currProgress <= m_FadeOut.context.destProgress;
                }
                else if (m_Addition.context.start)
                {
                    return m_Addition.context.currProgress >= m_Addition.context.destProgress;
                }
                else
                {
                    return m_FadeIn.context.currProgress >= m_FadeIn.context.destProgress;
                }
            }
            else if (IsDataAnimation())
            {
                if (animation == null) return true;
                else return animation.context.end;
            }
            return true;
        }

        public bool IsInDelay()
        {
            var anim = activedAnimation;
            if (anim != null)
                return anim.IsInDelay();
            return false;
        }

        /// <summary>
        /// アニメーションがデータアニメーションかどうか。BottomToTopとInsideOutはデータアニメーションです。
        /// </summary>
        public bool IsDataAnimation()
        {
            return context.type == AnimationType.BottomToTop || context.type == AnimationType.InsideOut;
        }

        /// <summary>
        /// アニメーションがシリーズアニメーションかどうか。LeftToRight、AlongPath、Clockwiseはシリーズアニメーションです。
        /// </summary>
        public bool IsSerieAnimation()
        {
            return context.type == AnimationType.LeftToRight ||
                context.type == AnimationType.AlongPath || context.type == AnimationType.Clockwise;
        }

        public bool CheckDetailBreak(float detail)
        {
            if (!IsSerieAnimation())
                return false;
            foreach (var animation in animations)
            {
                if (animation.context.start)
                    return !IsFinish() && detail > animation.context.currProgress;
            }
            return false;
        }

        public bool CheckDetailBreak(Vector3 pos, bool isYAxis)
        {
            if (!IsSerieAnimation())
                return false;

            if (IsFinish())
                return false;

            if (context.type == AnimationType.AlongPath)
            {
                context.currentPathDistance += Vector3.Distance(pos, m_LinePathLastPos);
                m_LinePathLastPos = pos;
                return CheckDetailBreak(context.currentPathDistance);
            }
            else
            {
                if (isYAxis)
                    return pos.y > GetCurrDetail();
                else
                    return pos.x > GetCurrDetail();
            }
        }

        public void CheckProgress()
        {
            if (IsDataAnimation() && context.isAllItemAnimationEnd)
            {
                foreach (var animation in animations)
                {
                    animation.End();
                }
                return;
            }
            foreach (var animation in animations)
            {
                animation.CheckProgress(animation.context.totalProgress, m_UnscaledTime);
            }
        }

        public void CheckProgress(double total)
        {
            if (IsFinish())
                return;
            foreach (var animation in animations)
            {
                animation.CheckProgress(total, m_UnscaledTime);
            }
        }

        internal float CheckItemProgress(int dataIndex, float destProgress, ref bool isEnd, float startProgress = 0)
        {
            isEnd = false;
            var anim = activedAnimation;
            if (anim == null)
            {
                isEnd = true;
                return destProgress;
            }
            return anim.CheckItemProgress(dataIndex, destProgress, ref isEnd, startProgress, m_UnscaledTime);
        }

        public void CheckSymbol(float dest)
        {
            m_FadeIn.CheckSymbol(dest, m_UnscaledTime);
            m_FadeOut.CheckSymbol(dest, m_UnscaledTime);
        }

        public float GetSysmbolSize(float dest)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return dest;
#endif
            if (!enable)
                return dest;

            if (IsEnd())
                return m_FadeOut.context.start ? 0 : dest;

            return m_FadeOut.context.start ? m_FadeOut.context.sizeProgress : m_FadeIn.context.sizeProgress;
        }

        public float GetCurrDetail()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                foreach (var animation in animations)
                {
                    if (animation.context.start)
                        return animation.context.destProgress;
                }
            }
#endif
            foreach (var animation in animations)
            {
                if (animation.context.start)
                    return animation.context.currProgress;
            }
            return m_FadeIn.context.currProgress;
        }

        public float GetCurrRate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return 1;
#endif
            if (!enable || IsEnd())
                return 1;
            return m_FadeOut.context.start ? m_FadeOut.context.currProgress : m_FadeIn.context.currProgress;
        }

        public int GetCurrIndex()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return -1;
#endif
            if (!enable)
                return -1;
            var anim = activedAnimation;
            if (anim == null)
                return -1;
            return (int)anim.context.currProgress;
        }

        public float GetChangeDuration()
        {
            if (m_Enable && m_Change.enable)
                return m_Change.context.currDuration > 0 ? m_Change.context.currDuration : m_Change.duration;
            else
                return 0;
        }

        public float GetExchangeDuration()
        {
            if (m_Enable && m_Exchange.enable)
                return m_Exchange.context.currDuration > 0 ? m_Exchange.context.currDuration : m_Exchange.duration;
            else
                return 0;
        }

        public float GetAdditionDuration()
        {
            if (m_Enable && m_Addition.enable)
                return m_Addition.context.currDuration > 0 ? m_Addition.context.currDuration : m_Addition.duration;
            else
                return 0;
        }

        public float GetInteractionDuration()
        {
            if (m_Enable && m_Interaction.enable)
                return m_Interaction.context.currDuration > 0 ? m_Interaction.context.currDuration : m_Interaction.duration;
            else
                return 0;
        }

        public float GetInteractionRadius(float radius)
        {
            if (m_Enable && m_Interaction.enable)
                return m_Interaction.GetRadius(radius);
            else
                return radius;
        }

        public bool HasFadeOut()
        {
            return enable && m_FadeOut.context.end;
        }

        public bool IsFadeIn()
        {
            return enable && m_FadeIn.context.start;
        }

        public bool IsFadeOut()
        {
            return enable && m_FadeOut.context.start;
        }

        public bool CanCheckInteract()
        {
            return enable && interaction.enable
                && !IsFadeIn() && !IsFadeOut();
        }
    }
}