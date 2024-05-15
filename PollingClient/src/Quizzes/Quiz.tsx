import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import Stack from 'react-bootstrap/Stack';
import Container from 'react-bootstrap/Container';
import Spinner from 'react-bootstrap/Spinner';
import Placeholder from 'react-bootstrap/Placeholder';
import PlaceholderImage from "./quiz-placeholder.jpg"; 
import "./Quiz.css";


export interface QuizDescriptor {
    id: number,
    title: string,
    description: string,
    createdAt: string,
    img: object,

};

function formateDateTime(dateStr: string): string {
    const date = new Date(dateStr);

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
}

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
                <img src={props.img != null ? props.img : PlaceholderImage} />
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
                </Card.Text>
                <span className="created-at text-secondary">
                    {!isLoaded ? (
                        <Placeholder animation="glow">
                            <Placeholder className='date' xs={6} /> <Placeholder className='time' xs={4} />{' '}
                        </Placeholder>
                    ) : (
                        formateDateTime(props.createdAt)
                    )}
                </span>
            </Card.Body>
        </Card>
    )
}

export default Quiz;