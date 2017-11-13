import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import Button from '../components/Button.jsx';

describe('Button', () => {

    let button;
    let text;
    let handleClick;    

    beforeEach(() => {
        handleClick = jest.fn();
        text = "text";
        button = mount(
            <Button handleClick={handleClick} text={text} />);
    });
    
    it('Button has a button', () => {
        expect(button.find('button').text()).toEqual(text);
    });

    it('Button requires handleClick prop', () => {
        expect(button.props().handleClick).toBeDefined();
    });

    it('Button requires fetchTextEntry prop', () => {
        expect(button.props().text).toBeDefined();
    });

    it('Button click calls handleClick', () => {
        const _button = button.find('button').first();
        _button.simulate('click');
        expect(handleClick).toBeCalled();        
    });
});