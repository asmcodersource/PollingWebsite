import { useState } from 'react';
import './QuestionEditor.css'
import Modal from 'react-bootstrap/Modal'
import SelectQuestionEditor from './SelectQuestionEditor';
import TextFieldQuestionEditor from './TextFieldQuestionEditor';

const QuestionEditor = (props) => {
    const [question, setQuestion] = useState(props.question);

    let questionComponent;

    switch (question.discriminator) {
        case 'TextFieldQuestion':
            questionComponent = <TextFieldQuestionEditor onHide={props.onHide} onUpdate={props.onUpdate} question={question} poll={props.poll} />;
            break;
        case 'SelectQuestion':
            questionComponent = <SelectQuestionEditor onHide={props.onHide} onUpdate={props.onUpdate} question={question} poll={props.poll} />;
            break;
        default:
            questionComponent = null;
    }
        
    return (
        <Modal className="question-editor" fullscreen={true} show={props.show} onHide={props.onHide}>
            <Modal.Header closeButton>
                <Modal.Title>Edit question</Modal.Title>
            </Modal.Header>
                <Modal.Body className="body">{questionComponent}</Modal.Body>
        </Modal>
    );
}

export default QuestionEditor;