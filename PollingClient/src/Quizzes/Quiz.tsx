import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import Stack from 'react-bootstrap/Stack';
import Container from 'react-bootstrap/Container';
import Spinner from 'react-bootstrap/Spinner';
import Placeholder from 'react-bootstrap/Placeholder'
import "./Quiz.css";

const Quiz = (props) => {
    let isLoaded: boolean = false;
    if (props.title != undefined)
        isLoaded = true;

    return (
        <Card className="quiz-wrapper">
            <Card.Title className="quiz-title">
                {!isLoaded ? (
                    <Placeholder as={Card.Text} animation="glow">
                        <Placeholder xs={6} />{' '}
                    </Placeholder>
                ) : (
                    props.title
                )}
            </Card.Title>
            {!isLoaded ? (
                <Container fluid className="img-wrapper">
                    <Spinner variant="secondary" animation="border" />
                </Container>
            ) : (
                <Card.Img variant="top" src={props.img} />
            )}
            <Card.Body> 
                <Card.Text className="quiz-description">
                    {!isLoaded ? (
                        <Placeholder as={Card.Text} animation="glow">
                            <Placeholder xs={2} /> <Placeholder xs={4} /> <Placeholder xs={1} /> <Placeholder xs={2} /> <Placeholder xs={4} />{' '}
                            <Placeholder xs={4} /> <Placeholder xs={2} /> <Placeholder xs={1} /> <Placeholder xs={8} /> <Placeholder xs={3} />
                            <Placeholder xs={8} /> <Placeholder xs={2} /> <Placeholder xs={1} /> <Placeholder xs={3} /> <Placeholder xs={3} />
                        </Placeholder>
                    ) : (
                        props.description
                    )}
                    <span className="created-at .text-secondary">
                        {!isLoaded ? (
                            <Placeholder animation="glow">
                                <Placeholder className='date' xs={6} /> <Placeholder className='time' xs={4} />{' '}
                            </Placeholder>
                        ) : (
                            <>{props.dateTime}</>
                        )}
                    </span>
                </Card.Text>
            </Card.Body>
        </Card>
    )
}

export default Quiz;