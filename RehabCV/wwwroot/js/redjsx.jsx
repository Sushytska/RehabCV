import React from 'react'
import { render } from 'react-dom'
import { createStore, applyMiddleware } from 'redux'
import thunk from 'redux-thunk'
import { Provider } from 'react-redux'
import rootReducer from './reducer'
import DemoApp from './DemoApp'
import './main.css'

let store = createStore(rootReducer, applyMiddleware(thunk))

document.addEventListener('DOMContentLoaded', function () {
    render(
        <Provider store={store}>
            <DemoApp />
        </Provider>,
        document.body.appendChild(document.createElement('div'))
    )
})
import { requestEventsInRange, requestEventCreate, requestEventUpdate, requestEventDelete } from './requests'

export default {

    toggleWeekends() {
        return {
            type: 'TOGGLE_WEEKENDS'
        }
    },

    requestEvents(startStr, endStr) {
        return (dispatch) => {
            return requestEventsInRange(startStr, endStr).then((plainEventObjects) => {
                dispatch({
                    type: 'RECEIVE_EVENTS',
                    plainEventObjects
                })
            })
        }
    },

    createEvent(plainEventObject) {
        return (dispatch) => {
            return requestEventCreate(plainEventObject).then((newEventId) => {
                dispatch({
                    type: 'CREATE_EVENT',
                    plainEventObject: {
                        id: newEventId,
                        ...plainEventObject
                    }
                })
            })
        }
    },

    updateEvent(plainEventObject) {
        return (dispatch) => {
            return requestEventUpdate(plainEventObject).then(() => {
                dispatch({
                    type: 'UPDATE_EVENT',
                    plainEventObject
                })
            })
        }
    },

    deleteEvent(eventId) {
        return (dispatch) => {
            return requestEventDelete(eventId).then(() => {
                dispatch({
                    type: 'DELETE_EVENT',
                    eventId
                })
            })
        }
    }

}
