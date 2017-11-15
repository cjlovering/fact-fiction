import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import Sentence from '../../components/Sentence.jsx';

describe('Sentence', () => {

    let id;
    let type;
    let content;
    let selectedEntryId;
    let selectEntry;
    let sentenceComponent;

    beforeEach(() => {
        id = '0';
        type = 'OBJECTIVE';
        selectedEntryId = "abc";
        selectEntry = jest.fn();
        content = 'This is a fact.';
        sentenceComponent = mount(
            <Sentence
                id={id}
                type={type}
                selectedEntryId={selectedEntryId}
                content={content}
                selectEntry={selectEntry}
            />);
    });

    it('Sentence requires id prop', () => {
        expect(sentenceComponent.props().id).toBeDefined();
    });

    it('Sentence requires type prop', () => {
        expect(sentenceComponent.props().type).toBeDefined();
    });

    it('Sentence component contains the right sentence', () => {
        expect(sentenceComponent.props().content).toBeDefined();
    });

    it('Sentence requires selectedEntryId prop', () => {
        expect(sentenceComponent.props().selectedEntryId).toBeDefined();
    });

    it('Click selects the highlighted sentence', () => {
        const _highlightedSpan = sentenceComponent.find('.highlighted').first();
        _highlightedSpan.simulate('click');
        expect(selectEntry).toBeCalledWith("0");
    });

    it('Subjective sentence is not highlighted', () => { 
        type = 'SUBJECTIVE';
        sentenceComponent = mount(
            <Sentence
                id={id}
                type={type}
                selectedEntryId={selectedEntryId}
                content={content}
                selectEntry={selectEntry}
            />);
        expect(sentenceComponent.find('.highlighted').exists()).toBe(false);
    });

    it('Other sentence is not highlighted', () => { 
        type = 'OTHER';
        sentenceComponent = mount(
            <Sentence
                id={id}
                type={type}
                selectedEntryId={selectedEntryId}
                content={content}
                selectEntry={selectEntry}
            />);
        expect(sentenceComponent.find('.highlighted').exists()).toBe(false);
    });

    it('If sentence is selected, className changes from .highlighted to .selected', () => { 
        id = 'abc';
        selectedEntryId = "abc";
        sentenceComponent = mount(
            <Sentence
                id={id}
                type={type}
                selectedEntryId={selectedEntryId}
                content={content}
                selectEntry={selectEntry}
            />);
        expect(sentenceComponent.find('.selected').exists()).toBe(true);
        expect(sentenceComponent.find('.highlighted').exists()).toBe(false);

        const _selectedSpan = sentenceComponent.find('.selected').first();
        _selectedSpan.simulate('click');
        expect(selectEntry).toBeCalledWith("abc");        
    });
});