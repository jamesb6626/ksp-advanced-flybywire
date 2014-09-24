﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XInputDotNetPure;

namespace KSPAdvancedFlyByWire
{
    public enum Button
    {
        DPadLeft = 0,
        DPadRight = 1,
        DPadUp = 2,
        DPadDown = 3,
        Back = 4,
        Start = 5,
        LeftShoulder = 6,
        RightShoulder = 7,
        X = 8,
        Y = 9,
        A = 10,
        B = 11,
        LeftStick = 12,
        RightStick = 13,
        Guide = 14
    }

    public enum AnalogInput
    {
        LeftStickX,
        LeftStickY,
        RightStickX,
        RightStickY,
        LeftTrigger,
        RightTrigger
    }

    public class ControllerWrapper
    {

        private GamePadState m_State;
        private PlayerIndex m_ControllerIndex = PlayerIndex.One;

        public delegate void ButtonPressedCallback(Button button, FlightCtrlState state);
        public delegate void ButtonReleasedCallback(Button button, FlightCtrlState state);

        public ButtonPressedCallback buttonPressedCallback = null;
        public ButtonReleasedCallback buttonReleasedCallback = null;
        public bool[] m_ButtonStates = new bool[15];

        public Curve analogInputEvaluationCurve = new Curve();
        public float analogInputDiscretizationCutoff = 0.5f;
        
        public ControllerWrapper()
        {
            for(int i = 0; i < 15; i++)
            {
                m_ButtonStates[i] = false;
            }
        }

        public void Update(FlightCtrlState state)
        {
            m_State = GamePad.GetState(m_ControllerIndex);

            for(int i = 0; i < 15; i++)
            {
                if(GetButton((Button)i) && !m_ButtonStates[i])
                {
                    m_ButtonStates[i] = true;

                    if(buttonPressedCallback != null)
                    {
                        buttonPressedCallback((Button)i, state);
                    }
                }
                else if(!GetButton((Button)i) && m_ButtonStates[i])
                {
                    m_ButtonStates[i] = false;

                    if(buttonReleasedCallback != null)
                    {
                        buttonReleasedCallback((Button)i, state);
                    }
                }
            }
        }

        public bool GetButton(Button button)
        {
            switch (button)
            {
                case Button.DPadLeft:
                    return m_State.DPad.Left == ButtonState.Pressed;
                case Button.DPadRight:
                    return m_State.DPad.Right == ButtonState.Pressed;
                case Button.DPadUp:
                    return m_State.DPad.Up == ButtonState.Pressed;
                case Button.DPadDown:
                    return m_State.DPad.Down == ButtonState.Pressed;
                case Button.Back:
                    return m_State.Buttons.Back == ButtonState.Pressed;
                case Button.Start:
                    return m_State.Buttons.Start == ButtonState.Pressed;
                case Button.LeftShoulder:
                    return m_State.Buttons.LeftShoulder == ButtonState.Pressed;
                case Button.RightShoulder:
                    return m_State.Buttons.RightShoulder == ButtonState.Pressed;
                case Button.X:
                    return m_State.Buttons.X == ButtonState.Pressed;
                case Button.Y:
                    return m_State.Buttons.Y == ButtonState.Pressed;
                case Button.A:
                    return m_State.Buttons.A == ButtonState.Pressed;
                case Button.B:
                    return m_State.Buttons.B == ButtonState.Pressed;
                case Button.LeftStick:
                    return m_State.Buttons.LeftStick == ButtonState.Pressed;
                case Button.RightStick:
                    return m_State.Buttons.RightStick == ButtonState.Pressed;
                case Button.Guide:
                    return m_State.Buttons.Guide == ButtonState.Pressed;
            }

            return false;
        }

        public float GetAnalogInput(AnalogInput input)
        {
            float value = 0.0f;

            switch (input)
            {
                case AnalogInput.LeftStickX:
                    value = m_State.ThumbSticks.Left.X;
                    break;
                case AnalogInput.LeftStickY:
                    value = m_State.ThumbSticks.Left.Y;
                    break;
                case AnalogInput.RightStickX:
                    value = m_State.ThumbSticks.Right.X;
                    break;
                case AnalogInput.RightStickY:
                    value = m_State.ThumbSticks.Right.Y;
                    break;
                case AnalogInput.LeftTrigger:
                    value = m_State.Triggers.Left;
                    break;
                case AnalogInput.RightTrigger:
                    value = m_State.Triggers.Right;
                    break;
            }

            return analogInputEvaluationCurve.Evaluate(value);
        }

        public bool GetDiscreteAnalogInput(AnalogInput input)
        {
            float value = 0.0f;

            switch (input)
            {
                case AnalogInput.LeftStickX:
                    value = m_State.ThumbSticks.Left.X;
                    break;
                case AnalogInput.LeftStickY:
                    value = m_State.ThumbSticks.Left.Y;
                    break;
                case AnalogInput.RightStickX:
                    value = m_State.ThumbSticks.Right.X;
                    break;
                case AnalogInput.RightStickY:
                    value = m_State.ThumbSticks.Right.Y;
                    break;
                case AnalogInput.LeftTrigger:
                    value = m_State.Triggers.Left;
                    break;
                case AnalogInput.RightTrigger:
                    value = m_State.Triggers.Right;
                    break;
            }

            return value >= analogInputDiscretizationCutoff;
        }

    }

}