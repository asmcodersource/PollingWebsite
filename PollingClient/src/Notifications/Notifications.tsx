import './Notifications.css'
import { useEffect, useState } from 'react';
import Notification from './Notification'
import Container from 'react-bootstrap/Container'

const placeholderNotifications = [
    {},
    {},
    {},
    {},
    {},
    {},
    {},
    {},
]

const Notifications = (props) => {
    const [notifications, setNotifications] = useState(placeholderNotifications) 
    return (
        <Container className="notifications-wrapper">
            {notifications.map((notification, index) => (
                <>
                    <Notification key={index} {...notification} />
                </>
            ))}
        </Container>
    );
}

export default Notifications;