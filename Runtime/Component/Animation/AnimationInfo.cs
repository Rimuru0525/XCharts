using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// アニメーションの設定情報。
    /// アニメーションの設定パラメータ。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationInfo
    {
        [SerializeField][Since("v3.8.0")] private bool m_Enable = true;
        [SerializeField][Since("v3.8.0")] private bool m_Reverse = false;
        [SerializeField][Since("v3.8.0")] private float m_Delay = 0;
        [SerializeField][Since("v3.8.0")] private float m_Duration = 1000;
        [SerializeField][Since("v3.14.0")] private float m_Speed = 0;
        public AnimationInfoContext context = new AnimationInfoContext();

        /// <summary>
        /// アニメーション効果を有効にするかどうか。
        /// アニメーション効果を有効にするかどうか。
        /// </summary>
        public bool enable { get { return m_Enable; } set { m_Enable = value; } }
        /// <summary>
        /// 逆方向アニメーションを有効にするかどうか。
        /// 逆方向アニメーションを有効にするかどうか。
        /// </summary>
        public bool reverse { get { return m_Reverse; } set { m_Reverse = value; } }
        /// <summary>
        /// アニメーション開始前の遅延時間。
        /// アニメーション開始前の遅延時間。
        /// </summary>
        public float delay { get { return m_Delay; } set { m_Delay = value; } }
        /// <summary>
        /// アニメーションの時間。
        /// アニメーションの時間。
        /// </summary>
        public float duration { get { return m_Duration; } set { m_Duration = value; } }
        /// <summary>
        /// アニメーションの速度。speedを指定すると、durationは無効になります。デフォルトは0で、速度は指定されません。
        /// </summary>
        public float speed { get { return m_Speed; } set { m_Speed = value; } }
        /// <summary>
        /// アニメーション開始時のコールバック関数。
        /// </summary>
        public Action OnAnimationStart { get; set; }
        /// <summary>
        /// アニメーション終了時のコールバック関数。
        /// </summary>
        public Action OnAnimationEnd { get; set; }

        /// <summary>
        /// アニメーション遅延のデリゲート関数。
        /// </summary>
        public AnimationDelayFunction delayFunction { get; set; }
        /// <summary>
        /// アニメーション時間のデリゲート関数。
        /// </summary>
        public AnimationDurationFunction durationFunction { get; set; }

        /// <summary>
        /// アニメーションをリセットします。
        /// </summary>
        public void Reset()
        {
            if (!enable) return;
            context.init = false;
            context.start = false;
            context.pause = false;
            context.end = false;
            context.startTime = 0;
            context.currProgress = 0;
            context.destProgress = 0;
            context.totalProgress = 0;
            context.sizeProgress = 0;
            context.currPointIndex = 0;
            context.currPoint = Vector3.zero;
            context.destPoint = Vector3.zero;
            context.dataCurrProgress.Clear();
            context.dataDestProgress.Clear();
        }

        /// <summary>
        /// アニメーションを開始します。
        /// </summary>
        /// <param name="reset">前回のパラメータをリセットするかどうか</param>
        public void Start(bool reset = true)
        {
            if (!enable) return;
            if (context.start)
            {
                context.pause = false;
                return;
            }
            context.init = false;
            context.start = true;
            context.end = false;
            context.pause = false;
            context.startTime = Time.time;
            if (reset)
            {
                context.currProgress = 0;
                context.destProgress = 1;
                context.totalProgress = 0;
                context.sizeProgress = 0;
                context.dataCurrProgress.Clear();
                context.dataDestProgress.Clear();
            }
            if (OnAnimationStart != null)
            {
                OnAnimationStart();
            }
        }

        /// <summary>
        /// アニメーションを一時停止します。
        /// </summary>
        public void Pause()
        {
            if (!enable) return;
            if (!context.start || context.end) return;
            context.pause = true;
        }

        /// <summary>
        /// アニメーションを再開します。
        /// </summary>
        public void Resume()
        {
            if (!enable) return;
            if (!context.pause) return;
            context.pause = false;
        }

        /// <summary>
        /// アニメーションを終了します。
        /// </summary>
        public void End()
        {
            if (!enable) return;
            if (!context.start || context.end) return;
            context.init = false;
            context.start = false;
            context.end = true;
            context.currPointIndex = context.destPointIndex;
            context.startTime = Time.time;
            if (OnAnimationEnd != null)
            {
                OnAnimationEnd();
            }
        }

        /// <summary>
        /// アニメーションを初期化します。
        /// </summary>
        /// <param name="curr">現在の進捗</param>
        /// <param name="dest">目標進捗</param>
        /// <param name="totalPointIndex">目標インデックス</param>
        /// <returns></returns>
        public bool Init(float curr, float dest, int totalPointIndex)
        {
            if (!enable || !context.start) return false;
            context.totalProgress = dest - curr;
            context.destPointIndex = totalPointIndex;
            if (reverse)
            {
                if (!context.init) context.currProgress = dest;
                context.destProgress = curr;
            }
            else
            {
                if (!context.init) context.currProgress = curr;
                context.destProgress = dest;
            }
            context.init = true;
            return true;
        }

        /// <summary>
        /// アニメーションが終了したかどうか。
        /// </summary>
        public bool IsFinish()
        {
            if (!context.start) return true;
            if (context.end) return true;
            if (context.pause) return false;
            if (!context.init) return false;
            return m_Reverse ? context.currProgress <= context.destProgress
                : context.currProgress >= context.destProgress;
        }

        /// <summary>
        /// アニメーションが遅延中かどうか。
        /// </summary>
        public bool IsInDelay()
        {
            if (!context.start)
                return false;
            else
                return m_Delay > 0 && Time.time - context.startTime < m_Delay / 1000;
        }

        /// <summary>
        /// アニメーションがインデックス遅延中かどうか。
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public bool IsInIndexDelay(int dataIndex)
        {
            if (context.start)
                return Time.time - context.startTime < GetIndexDelay(dataIndex) / 1000f;
            else
                return false;
        }

        /// <summary>
        /// アニメーション遅延を取得します。
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
        public float GetIndexDelay(int dataIndex)
        {
            if (!context.start) return 0;
            if (delayFunction != null)
                return delayFunction(dataIndex);
            return delay;
        }

        internal float GetCurrAnimationDuration(int dataIndex = -1)
        {
            if (dataIndex >= 0)
            {
                if (context.start && durationFunction != null)
                    return durationFunction(dataIndex) / 1000f;
            }
            return m_Duration > 0 ? m_Duration / 1000 : 1f;
        }

        internal void SetDataCurrProgress(int index, float state)
        {
            context.dataCurrProgress[index] = state;
        }


        internal float GetDataCurrProgress(int index, float initValue, float destValue, ref bool isBarEnd)
        {
            if (IsInDelay())
            {
                isBarEnd = false;
                return initValue;
            }
            var c1 = !context.dataCurrProgress.ContainsKey(index);
            var c2 = !context.dataDestProgress.ContainsKey(index);
            if (c1 || c2)
            {
                if (c1)
                    context.dataCurrProgress.Add(index, initValue);

                if (c2)
                    context.dataDestProgress.Add(index, destValue);

                isBarEnd = false;
            }
            else
            {
                isBarEnd = context.dataCurrProgress[index] == context.dataDestProgress[index];
            }
            return context.dataCurrProgress[index];
        }

        internal void CheckProgress(double total, bool m_UnscaledTime)
        {
            if (!context.start || !context.init || context.pause) return;
            if (IsInDelay()) return;
            var delta = GetDelta(total, m_UnscaledTime);
            if (reverse)
            {
                context.currProgress -= delta;
                if (context.currProgress <= context.destProgress)
                {
                    context.currProgress = context.destProgress;
                    End();
                }
            }
            else
            {
                context.currProgress += delta;
                if (context.currProgress >= context.destProgress)
                {
                    context.currProgress = context.destProgress;
                    End();
                }
            }
        }

        internal float CheckItemProgress(int dataIndex, float destProgress, ref bool isEnd, float startProgress, bool m_UnscaledTime)
        {
            if (m_Reverse)
            {
                var temp = startProgress;
                startProgress = destProgress;
                destProgress = temp;
            }
            var currHig = GetDataCurrProgress(dataIndex, startProgress, destProgress, ref isEnd);
            if (IsFinish())
            {
                return destProgress;
            }
            else if (IsInDelay() || IsInIndexDelay(dataIndex))
            {
                return startProgress;
            }
            else if (context.pause)
            {
                return currHig;
            }
            else
            {
                var delta = GetDelta(destProgress - startProgress, m_UnscaledTime);
                currHig += delta;
                if (reverse)
                {
                    if ((destProgress > 0 && currHig <= 0) || (destProgress < 0 && currHig >= 0))
                    {
                        currHig = 0;
                        isEnd = true;
                    }
                }
                else
                {
                    if ((destProgress - startProgress > 0 && currHig > destProgress) ||
                        (destProgress - startProgress < 0 && currHig < destProgress))
                    {
                        currHig = destProgress;
                        isEnd = true;
                    }
                }
                SetDataCurrProgress(dataIndex, currHig);
                return currHig;
            }
        }

        internal void CheckSymbol(float dest, bool m_UnscaledTime)
        {
            if (!context.start || !context.init || context.pause) return;

            if (IsInDelay())
                return;

            var delta = GetDelta(dest, m_UnscaledTime);
            if (reverse)
            {
                context.sizeProgress -= delta;
                if (context.sizeProgress < 0)
                    context.sizeProgress = 0;
            }
            else
            {
                context.sizeProgress += delta;
                if (context.sizeProgress > dest)
                    context.sizeProgress = dest;
            }
        }

        private float GetDelta(double total, bool unscaledTime)
        {
            if (m_Speed > 0)
            {
                context.currDuration = (float)total / m_Speed;
                return (float)(m_Speed * (unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime));
            }
            else
            {
                context.currDuration = 0;
                return (float)(total / GetCurrAnimationDuration() * (unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime));
            }
        }
    }

    /// <summary>
    /// フェードインアニメーション。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationFadeIn : AnimationInfo
    {
    }

    /// <summary>
    /// フェードアウトアニメーション。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationFadeOut : AnimationInfo
    {
    }

    /// <summary>
    /// データ変更アニメーション。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationChange : AnimationInfo
    {
    }

    /// <summary>
    /// データ追加アニメーション。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationAddition : AnimationInfo
    {
    }

    /// <summary>
    /// データ非表示アニメーション。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationHiding : AnimationInfo
    {
    }

    /// <summary>
    /// チャートのインタラクションアニメーション。
    /// </summary>
    [Since("v3.8.0")]
    [System.Serializable]
    public class AnimationInteraction : AnimationInfo
    {
        [SerializeField][Since("v3.8.0")] private MLValue m_Width = new MLValue(1.1f);
        [SerializeField][Since("v3.8.0")] private MLValue m_Radius = new MLValue(1.1f);
        [SerializeField][Since("v3.8.0")] private MLValue m_Offset = new MLValue(MLValue.Type.Absolute, 5f);

        /// <summary>
        /// 幅のML値。
        /// </summary>
        public MLValue width { get { return m_Width; } set { m_Width = value; } }
        /// <summary>
        /// 半径のML値。
        /// </summary>
        public MLValue radius { get { return m_Radius; } set { m_Radius = value; } }
        /// <summary>
        /// オフセットのML値。例：円グラフのセクター選択時のオフセット。
        /// </summary>
        public MLValue offset { get { return m_Offset; } set { m_Offset = value; } }

        public float GetRadius(float radius)
        {
            return m_Radius.GetValue(radius);
        }

        public float GetWidth(float width)
        {
            return m_Width.GetValue(width);
        }

        public float GetOffset(float total)
        {
            return m_Offset.GetValue(total);
        }

        public float GetOffset()
        {
            return m_Offset.value;
        }
    }

    /// <summary>
    /// データ交換アニメーション。主にデータソート時の順序変更アニメーションで使用。
    /// </summary>
    [Since("v3.15.0")]
    [System.Serializable]
    public class AnimationExchange : AnimationInfo
    {
    }
}