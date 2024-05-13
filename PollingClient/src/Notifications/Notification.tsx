import './Notification.css'
import Placeholder from 'react-bootstrap/Placeholder'
import { useEffect, useState } from 'react';
import Card from 'react-bootstrap/Card'

const Notification = (props) => {
    let isLoaded: boolean = false;
    if (props.title != undefined)
        isLoaded = true;

    return (
        <Card className="notification-wrapper">
            <Card.Body>
                <label className="title">
                    {!isLoaded ?
                        (
                            <Placeholder animation="glow">
                                <Placeholder xs={6} />
                            </Placeholder>
                        )
                        :
                        (
                            props.title
                        )
                    }
                </label>
                <p className="message">
                    <Placeholder animation="glow">
                        <Placeholder xs={6} /> <Placeholder xs={2} /> <Placeholder xs={2} /> {' '}
                    </Placeholder>
                </p>
                <span className="created-at text-secondary">
                    {!isLoaded ? (
                        <Placeholder animation="glow">
                            <Placeholder className='date' xs={6} /> <Placeholder className='time' xs={4} />{' '}
                        </Placeholder>
                    ) : (
                        <>{props.dateTime}</>
                    )}
                </span>
            </Card.Body>
        </Card>
    )
}

export default Notification;