#pragma once




#include "Common\DeviceResources.h"
#include "Common\StepTimer.h"

#include "Content\SpinningCubeRenderer.h"
#include "Content\SpatialInputHandler.h"

namespace HololensAssistant
{



	//-------------------------------------------------------------------------








	 ///-----------------------------------------------------------------------
	///
	/// Девайс хололенса
	/// 
	///
	///------------------------------------------------------------------------
    class HololensAssistantMain : public DX::IDeviceNotify
    {
    public:
        HololensAssistantMain(const std::shared_ptr<DX::DeviceResources>& deviceResources);
        ~HololensAssistantMain();

        // Sets the holographic space. This is our closest analogue to setting a new window
        // for the app.
        void SetHolographicSpace(Windows::Graphics::Holographic::HolographicSpace^ holographicSpace);

        // Starts the holographic frame and updates the content.
        Windows::Graphics::Holographic::HolographicFrame^ Update();

        // Renders holograms, including world-locked content.
        bool Render(Windows::Graphics::Holographic::HolographicFrame^ holographicFrame);

        // Handle saving and loading of app state owned by AppMain.
        void SaveAppState();
        void LoadAppState();

        // Handle mouse input.
        void OnPointerPressed();

        // IDeviceNotify
        virtual void OnDeviceLost();
        virtual void OnDeviceRestored();

    private:
        // Asynchronously creates resources for new holographic cameras.
        void OnCameraAdded(
            Windows::Graphics::Holographic::HolographicSpace^ sender,
            Windows::Graphics::Holographic::HolographicSpaceCameraAddedEventArgs^ args);

        // Synchronously releases resources for holographic cameras that are no longer
        // attached to the system.
        void OnCameraRemoved(
            Windows::Graphics::Holographic::HolographicSpace^ sender,
            Windows::Graphics::Holographic::HolographicSpaceCameraRemovedEventArgs^ args);

        // Used to notify the app when the positional tracking state changes.
        void OnLocatabilityChanged(
            Windows::Perception::Spatial::SpatialLocator^ sender,
            Platform::Object^ args);

        // Used to be aware of gamepads that are plugged in after the app starts.
        void OnGamepadAdded(Platform::Object^, Windows::Gaming::Input::Gamepad^ args);

        // Used to stop looking for gamepads that are removed while the app is running.
        void OnGamepadRemoved(Platform::Object^, Windows::Gaming::Input::Gamepad^ args);

        // Clears event registration state. Used when changing to a new HolographicSpace
        // and when tearing down AppMain.
        void UnregisterHolographicEventHandlers();

#ifdef DRAW_SAMPLE_CONTENT
        // Renders a colorful holographic cube that's 20 centimeters wide. This sample content
        // is used to demonstrate world-locked rendering.
        std::unique_ptr<SpinningCubeRenderer>                           m_spinningCubeRenderer;

        // Listens for the Pressed spatial input event.
        std::shared_ptr<SpatialInputHandler>                            m_spatialInputHandler;
#endif

        // Cached pointer to device resources.
        std::shared_ptr<DX::DeviceResources>                            m_deviceResources;

        // Render loop timer.
        DX::StepTimer                                                   m_timer;

        // Represents the holographic space around the user.
        Windows::Graphics::Holographic::HolographicSpace^               m_holographicSpace;

        // SpatialLocator that is attached to the primary camera.
        Windows::Perception::Spatial::SpatialLocator^                   m_locator;

        // A reference frame attached to the holographic camera.
        Windows::Perception::Spatial::SpatialStationaryFrameOfReference^ m_referenceFrame;

        // Event registration tokens.
        Windows::Foundation::EventRegistrationToken                     m_cameraAddedToken;
        Windows::Foundation::EventRegistrationToken                     m_cameraRemovedToken;
        Windows::Foundation::EventRegistrationToken                     m_locatabilityChangedToken;
        Windows::Foundation::EventRegistrationToken                     m_gamepadAddedEventToken;
        Windows::Foundation::EventRegistrationToken                     m_gamepadRemovedEventToken;

        // Keep track of gamepads.
        struct GamepadWithButtonState
        {
            GamepadWithButtonState(
                Windows::Gaming::Input::Gamepad^ gamepad,
                bool buttonAWasPressedLastFrame)
            {
                this->gamepad = gamepad;
                this->buttonAWasPressedLastFrame = buttonAWasPressedLastFrame;
            }
            Windows::Gaming::Input::Gamepad^ gamepad;
            bool buttonAWasPressedLastFrame = false;
        };
        std::vector<GamepadWithButtonState>                             m_gamepads;

        // Keep track of mouse input.
        bool                                                            m_pointerPressed = false;
    };
}
