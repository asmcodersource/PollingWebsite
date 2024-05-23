import { useState, useRef} from 'react';
import './QuestionEditor.css'
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal'
import SelectQuestionEditor from './SelectQuestionEditor';
import TextFieldQuestionEditor from './TextFieldQuestionEditor';
import { BaseQuestion } from '../../../Questions/QuizQuestion';


export interface ExternalSignal {
    handler: Function
}

export function putQuestionToServer(question: BaseQuestion, pollId: number) {
    return new Promise(async (resolve, reject) => {
        let response: Response | null = null;
        try {
            response = await fetch(`/api/poll/${pollId}/question`, {
                method: "PUT",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + sessionStorage.getItem('token')
                },
                body: JSON.stringify(
                    { ...question }
                )
            });
            if (response.status == 200)
                resolve(response);
            else
                reject(response);
        } catch {
            reject(response);
        }
    });
}

const QuestionEditor = (props) => {
    const [saveButtonSignal, _] = useState<ExternalSignal>({ handler: function () { } });
    const [question, setQuestion] = useState(props.question);

    let questionComponent;

    switch (question.discriminator) {
        case 'TextFieldQuestion':
            questionComponent = <TextFieldQuestionEditor saveButtonSignal={saveButtonSignal} onHide={props.onHide} onUpdate={props.onUpdate} question={question} poll={props.poll} />;
            break;
        case 'SelectQuestion':
            questionComponent = <SelectQuestionEditor saveButtonSignal={saveButtonSignal} onHide={props.onHide} onUpdate={props.onUpdate} question={question} poll={props.poll} />;
            break;
        default:
            questionComponent = null;
    }
        
    return (
        <Modal className="question-editor" fullscreen={true} show={props.show} onHide={props.onHide}>
            <Modal.Header closeButton>
                <Modal.Title>Edit question</Modal.Title>
            </Modal.Header>
            <Modal.Body className="body">
                <div className="limiter-container-wrapper">
                    <div className="limiter-container">
                        {questionComponent}
                    </div>
                </div>
            </Modal.Body>
            <Modal.Footer>
                <Button
                    variant="primary"
                    className="add-question-button"
                    onClick={() => saveButtonSignal?.handler() }
                >
                    Save
                </Button>
            </Modal.Footer>
        </Modal>
    );
}

export default QuestionEditor;